using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class LogWindow : Form
    {
        // Rule sets for this log window
        private List<ParseRule> parseRules;
        private List<HighlightRule> highlightRules;

        public List<ParseRule> ParseRules => this.parseRules.ToList();
        public List<HighlightRule> HighlightRules => this.highlightRules.ToList();

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

            // Find a match
            // TODO: this should only result in a single Rule/Match set, a log message shouldn't be able to create multiple lines.
            foreach (ParseRule rule in this.parseRules)
            {
                Match match = Regex.Match(message.Text, rule.Expression, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    message.Handled = true;

                    // Build DGV row
                    DataGridViewRow row = this.dgvLogMessages.Rows[this.dgvLogMessages.Rows.Add()];
                    foreach (ColumnDefinition columnDefinition in this.columns)
                        row.Cells[columnDefinition.ColumnName].Value = match.Groups[columnDefinition.ColumnName].Value;

                    // Highlight row
                    HighlightRule highlightRule = this.highlightRules.Find(r => Regex.IsMatch(message.Text, r.Expression));
                    if (highlightRule != null)
                        row.DefaultCellStyle = new DataGridViewCellStyle()
                        {
                            BackColor = Color.FromKnownColor(highlightRule.BackgroundColor),
                            Font = new Font(Font,
                                (highlightRule.Bold ? FontStyle.Bold : FontStyle.Regular)
                                | (highlightRule.Italic ? FontStyle.Italic : FontStyle.Regular)
                                | (highlightRule.Underline ? FontStyle.Underline : FontStyle.Regular)
                            ),
                            ForeColor = Color.FromKnownColor(highlightRule.ForegroundColor)
                        };

                    break;
                }
            }

            // Scroll to the bottom, if necessary
            if (this.dgvLogMessages.Rows.Count > 0 && scroll && message.Handled)
                this.dgvLogMessages.FirstDisplayedScrollingRowIndex = this.dgvLogMessages.Rows[this.dgvLogMessages.Rows.Count - 1].Index;
        }

        #region Editor
        private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ColumnDefinition> editorResult = this.showEditor<ColumnDefinition>("Edit columns", this.columns);
            this.columns = editorResult ?? this.columns;
            
            // Update only if OK was pressed
            if (editorResult != null)
                this.updateColumns();

            this.Updated?.Invoke(this);
        }

        private void editParseRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.parseRules = this.showEditor<ParseRule>("Edit parse rules", this.ParseRules) ?? this.ParseRules;
            this.Updated?.Invoke(this);
        }

        private void editHighlightRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.highlightRules = this.showEditor<HighlightRule>("Edit highlighting rules", this.highlightRules) ?? this.highlightRules;
            this.Updated?.Invoke(this);
        }

        private List<T> showEditor<T>(string title, List<T> data)
        {
            Editor<T> editor = new Editor<T>(data);
            editor.Text = title;
            if (editor.ShowDialog() != DialogResult.OK)
                return null;
            return editor.Data;
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
