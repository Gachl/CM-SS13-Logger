using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private UnhandledMessages unhandledMessages;
        private bool hideErrorMessages = false;
        private LogReader logReader;

        private string selectLogFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "CM-SS13 Live Log File (*.htm)|*.htm"
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.statusStrip_Resize(sender, e);

            // Force the window to be on top for agressive prompt
            this.BringToFront();

            // Prompt for log file
            DialogResult result = DialogResult.No;
            string logFilePath = null;
            while (result == DialogResult.No)
            {
                logFilePath = this.selectLogFile();
                if (!string.IsNullOrEmpty(logFilePath))
                    break;

                result = MessageBox.Show("No valid log file has been selected, would you like to exit?", "No log file", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                    return;
                }
            }

            // Initialise log reader
            result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    this.logReader = new LogReader(logFilePath);
                    break;
                }
                catch (Exception ex)
                {
                    result = MessageBox.Show($"An error occured while trying to open the log file for reading. Please use the following error message to resolve the issue and retry:{Environment.NewLine}{ex.Message}", ex.GetType().Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Cancel)
                    {
                        this.Close();
                        return;
                    }
                }
            }

            this.logReader.Exception += LogReader_Exception;
            this.logReader.Progress += LogReader_Progress;

            // Initialise windows
            this.loadWindowsFromSettings();

            // Tile windows
            this.tileHorizontalToolStripMenuItem_Click(sender, e);

            // Unhandled messages window
            this.unhandledMessages = new UnhandledMessages(this.logReader) { MdiParent = this };

            // Prompt for skipping to end of log
            this.logReader.Run(MessageBox.Show("Do you want to skip to the end of the log file?", "Skip past log", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes);
        }

        private void LogReader_Progress(float progress)
        {
            // Cross-thread invokation
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.LogReader_Progress(progress)));
                return;
            }

            this.progressBar.Value = (int)(progress * 1000.0f);
            this.lblCurrentStatus.Text = $"Parsed {this.logReader.ParsedMessages} / {this.logReader.TotalMessages} messages";
        }

        private void LogReader_Exception(Exception e)
        {
            if (this.hideErrorMessages)
                return;

            // Cross-thread invokation
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.LogReader_Exception(e)));
                return;
            }

            this.hideErrorMessages = MessageBox.Show($"An error occured while parsing the log file. Please use the following error message to resolve the issue:{Environment.NewLine}{e.Message}{Environment.NewLine}Would you like to hide further error messages?", e.GetType().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        #region Settings file
        private void loadWindowsFromSettings()
        {
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    foreach (WindowSettings windowSetting in JsonConvert.DeserializeObject<WindowSettings[]>(File.ReadAllText("./windowsettings.json")))
                    {
                        LogWindow logWindow = new LogWindow(this.logReader, windowSetting.Columns, windowSetting.ParseRules, windowSetting.HighlightRules);
                        logWindow.MdiParent = this;
                        logWindow.Updated += LogWindow_Updated;
                        logWindow.Text = windowSetting.WindowName;
                        logWindow.Show();
                    }
                    break;
                }
                catch (FileNotFoundException) { }
                catch (Exception e)
                {
                    result = MessageBox.Show($"An error occured while trying to open or parse the settings file. Please use the following error message to resolve the issue and retry or ignore to reset the file:{Environment.NewLine}{e.Message}", e.GetType().Name, MessageBoxButtons.AbortRetryIgnore);
                    if (result == DialogResult.Abort)
                        throw;
                }
            }
        }

        private void LogWindow_Updated(LogWindow source)
        {
            this.saveWindowsToSettings();
        }

        private void saveWindowsToSettings()
        {
            File.WriteAllText("./windowsettings.json",
                JsonConvert.SerializeObject(this.MdiChildren.OfType<LogWindow>().Select(w => new WindowSettings()
                {
                    WindowName = w.Text,
                    HighlightRules = w.HighlightRules.ToArray(),
                    ParseRules = w.ParseRules.ToArray(),
                    Columns = w.Columns.ToArray()
                }).ToArray()));
        }
        #endregion

        #region Window arrangement
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        #endregion

        // Create a new window
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogWindow logWindow = new LogWindow(this.logReader);
            logWindow.MdiParent = this;
            logWindow.Text = "New Log Window";
            logWindow.Updated += this.LogWindow_Updated;
            logWindow.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveWindowsToSettings();
        }

        private class WindowSettings
        {
            public string WindowName { get; set; }
            public ParseRule[] ParseRules { get; set; }
            public HighlightRule[] HighlightRules { get; set; }
            public ColumnDefinition[] Columns { get; set; }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string unstableWarning = version.EndsWith(".0.0") ? "" : $"{Environment.NewLine}{Environment.NewLine}NOTICE:{Environment.NewLine}You are using an unstable version of this software. Please use it only for testing purposes.";
            MessageBox.Show($"Thank you for using CM-SS13-Logger v{version}{Environment.NewLine}Please report bugs, ideas or question to Gachl#4156 on Discord.{Environment.NewLine}The author of this software does not take responsibility for any damages caused by using it.{unstableWarning}", "About", MessageBoxButtons.OK);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showUnhandledMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.unhandledMessages.Show();
        }

        private void statusStrip_Resize(object sender, EventArgs e)
        {
            this.progressBar.Width = this.statusStrip.Width - this.lblCurrentStatus.Width - 32;
        }

        private void stayTopmostWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            this.stayTopmostWindowToolStripMenuItem.Text = this.TopMost ? "Don't stay topmost window" : "Stay topmost window";
        }
    }
}
