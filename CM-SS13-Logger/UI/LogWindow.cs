using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class LogWindow : Form
    {
        private static readonly int MAX_HISTORY = 20;

        // Rule sets for this log window
        private List<ParseRule> parseRules;
        private List<HighlightRule> highlightRules;

        public List<ParseRule> ParseRules => this.parseRules.ToList();
        public List<HighlightRule> HighlightRules => this.highlightRules.ToList();

        private Queue<string> lineHistory;

        // Font cache required for performance and GC reasons
        private List<Font> fontCache = new List<Font>();

        // Columns
        private List<ColumnDefinition> columns;
        public List<ColumnDefinition> Columns => this.columns.Select(c => new ColumnDefinition()
        {
            ColumnLabel = c.ColumnLabel,
            ColumnName = c.ColumnName,
            Width = this.dgvLogMessages.Columns[c.ColumnName].Width
        }).ToList();

        // Updated event
        public delegate void OnUpdated(LogWindow source);
        public event OnUpdated Updated;

        // Constructors
        public LogWindow(LogReader logReader)
        {
            if (logReader == null)
                throw new ArgumentNullException("logReader");

            this.initialize(logReader);
        }

        public LogWindow(LogReader logReader, ColumnDefinition[] columns, ParseRule[] parseRules, HighlightRule[] highlightRules)
        {
            if (logReader == null)
                throw new ArgumentNullException("logReader");
            if (parseRules == null)
                throw new ArgumentNullException("parseRules");
            if (highlightRules == null)
                throw new ArgumentNullException("highlightRules");
            if (columns == null)
                throw new ArgumentNullException("columns");

            this.initialize(logReader, columns, parseRules, highlightRules);
        }

        // Initialisation
        private void initialize(LogReader logReader, ColumnDefinition[] columns = null, ParseRule[] parseRules = null, HighlightRule[] highlightRules = null)
        {
            // Form initialisation
            InitializeComponent();
            this.dgvLogMessages.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Setup rules
            this.columns = columns?.ToList() ?? new List<ColumnDefinition>();
            this.parseRules = parseRules?.ToList() ?? new List<ParseRule>();
            this.highlightRules = highlightRules?.ToList() ?? new List<HighlightRule>();

            // Setup message handling
            this.lineHistory = new Queue<string>();
            logReader.LogMessage += onLogMessage;

            // Column initialisation
            this.updateColumns();
        }

        private void updateColumns()
        {
            // Cross-thread invokation
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.updateColumns()));
                return;
            }

            // Clear data
            this.dgvLogMessages.Rows.Clear();
            this.dgvLogMessages.Columns.Clear();

            // Add columns
            foreach (ColumnDefinition column in this.columns)
                this.dgvLogMessages.Columns[this.dgvLogMessages.Columns.Add(column.ColumnName, column.ColumnLabel)].Width = column.Width == 0 ? 100 : column.Width;
        }

        private void onLogMessage(Message message)
        {
            // Cross-thread invokation
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.onLogMessage(message)));
                return;
            }

            // Check if we should be scrolling before adding a new row
            bool scroll = this.dgvLogMessages.Rows.Count == 0 || this.dgvLogMessages.Rows[this.dgvLogMessages.Rows.Count - 1].Displayed;

            // Keep track of the history, limited for performance reasons
            this.lineHistory.Enqueue(message.Text);
            while (this.lineHistory.Count > MAX_HISTORY)
                this.lineHistory.Dequeue();

            // Find a match
            foreach (ParseRule rule in this.parseRules)
            {
                string text = rule.Multiline ? String.Join(Environment.NewLine, this.lineHistory) : message.Text;
                Match match = Regex.Match(text, rule.Expression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    message.Handled = true;

                    // Build DGV row
                    DataGridViewRow row = this.dgvLogMessages.Rows[this.dgvLogMessages.Rows.Add()];
                    foreach (ColumnDefinition columnDefinition in this.columns)
                        if (this.dgvLogMessages.Columns.Contains(columnDefinition.ColumnName) && match.Groups.Cast<Group>().ToList().Find(g => g.Name == columnDefinition.ColumnName) != null)
                            row.Cells[columnDefinition.ColumnName].Value = match.Groups[columnDefinition.ColumnName].Value;

                    // Highlight row
                    // TODO: performance gets horrible the more rows have a font applied, even with the font cache
                    FontStyle style = (row.DefaultCellStyle.Font ?? this.dgvLogMessages.Font).Style;
                    foreach (HighlightRule highlightRule in this.highlightRules.Where(r => Regex.IsMatch(text, r.Expression)))
                    {

                        if (highlightRule.ForegroundColor.HasValue)
                            row.DefaultCellStyle.ForeColor = Color.FromKnownColor(highlightRule.ForegroundColor.Value);
                        if (highlightRule.BackgroundColor.HasValue)
                            row.DefaultCellStyle.BackColor = Color.FromKnownColor(highlightRule.BackgroundColor.Value);

                        if (highlightRule.Bold != CheckState.Indeterminate)
                            style = highlightRule.Bold == CheckState.Checked ? style | FontStyle.Bold : style & ~FontStyle.Bold;
                        if (highlightRule.Italic != CheckState.Indeterminate)
                            style = highlightRule.Italic == CheckState.Checked ? style | FontStyle.Italic : style & ~FontStyle.Italic;
                        if (highlightRule.Underline != CheckState.Indeterminate)
                            style = highlightRule.Underline == CheckState.Checked ? style | FontStyle.Underline : style & ~FontStyle.Underline;

                        if (highlightRule.Stop)
                            break;
                    }

                    // If no style has been applied, don't update the font
                    if ((row.DefaultCellStyle.Font ?? this.dgvLogMessages.Font).Style == style)
                        break;

                    Font cachedFont = this.fontCache.Find(f => f.Style == style);
                    if (cachedFont == null)
                    {
                        cachedFont = new Font(row.DefaultCellStyle.Font ?? this.dgvLogMessages.Font, style);
                        this.fontCache.Add(cachedFont);
                    }

                    row.DefaultCellStyle.Font = cachedFont;
                    break;
                }
            }

            // Scroll to the bottom, if necessary
            try
            {
                if (this.dgvLogMessages.Rows.Count > 0 && scroll && message.Handled)
                    this.dgvLogMessages.FirstDisplayedScrollingRowIndex = this.dgvLogMessages.Rows[this.dgvLogMessages.Rows.Count - 1].Index;
            }
            // TODO: figure out which tests need to be performed before calling FirstDisplayedScrollingRowIndex to stop exceptions
            catch (Exception) { }
        }

        #region Editor
        private Dictionary<string, Form> editors = new Dictionary<string, Form>();

        private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showEditor<ColumnDefinition>("Edit columns", this.columns, ed =>
            {
                List<ColumnDefinition> editorResult = ed.Data;

                this.columns = editorResult ?? this.columns;

                this.updateColumns();

                this.Updated?.Invoke(this);
            });
        }

        private void editParseRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showEditor<ParseRule>("Edit parse rules", this.ParseRules, ed =>
            {
                this.parseRules = ed.Data ?? this.ParseRules;
                this.Updated?.Invoke(this);
            });
        }

        private void editHighlightRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showEditor<HighlightRule>("Edit highlighting rules", this.highlightRules, ed =>
            {
                this.highlightRules = ed.Data ?? this.highlightRules;
                this.Updated?.Invoke(this);
            });
        }

        private void showEditor<T>(string title, List<T> data, Action<Editor<T>> callback)
        {
            // Prevent multiple instances of the same editor
            if (this.editors.ContainsKey(title))
            {
                this.editors[title].Show();
                this.editors[title].BringToFront();
                return;
            }

            // Create and show editor
            Editor<T> editor = new Editor<T>(data);
            editor.Text = title;
            editor.TopMost = this.MdiParent.TopMost;
            editor.Show();
            editor.FormClosed += (s, e) =>
            {
                this.editors.Remove(title);

                if (editor.DialogResult != DialogResult.OK)
                    return;

                callback(editor);
            };
            this.editors.Add(title, editor);
        }
        #endregion

        private void renameWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowTitlePrompt windowTitlePrompt = new WindowTitlePrompt(this.Text);
            if (windowTitlePrompt.ShowDialog() != DialogResult.OK)
                return;
            this.Text = windowTitlePrompt.WindowTitle;
            this.Updated?.Invoke(this);
        }
    }
}
