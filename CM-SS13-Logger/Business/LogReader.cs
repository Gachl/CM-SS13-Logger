using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CM_SS13_Logger
{
    public class LogReader
    {
        // Log file path
        private readonly string path;

        // Message event
        public delegate void OnLogMessage(Message message);
        public event OnLogMessage LogMessage;

        // Exception event
        public delegate void OnException(Exception e);
        public event OnException Exception;

        // Progress event
        public delegate void OnProgress(float progress);
        public event OnProgress Progress;

        // Unhandled messages
        private List<string> unhandledMessages = new List<string>();

        public List<string> UnhandledMessages => this.unhandledMessages.ToList();

        // Statistics
        public int ParsedMessages { get; private set; }
        public int TotalMessages { get; private set; }

        // Constructor
        public LogReader(string path)
        {
            this.path = path;

            // Try to open the file for reading or let the caller handle the exception
            File.Open(this.path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite).Dispose();
        }

        // Execution of the reader thread
        public void Run(bool skipToEnd)
        {
            Task.Run(async () => await this.reader(skipToEnd));
        }

        // Reader thread
        private async Task reader(bool skipToEnd)
        {
            try
            {
                using (FileStream fileStream = File.Open(this.path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        if (skipToEnd)
                            await reader.ReadToEndAsync();

                        // Handle log lines
                        while (true)
                        {
                            string[] messages = (await reader.ReadToEndAsync())?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).ToArray();

                            this.TotalMessages += messages.Length;

                            foreach (string messageText in messages)
                            {
                                if (!string.IsNullOrEmpty(messageText))
                                {
                                    Message message = new Message() { Text = HttpUtility.HtmlDecode(messageText), Handled = false };
                                    this.LogMessage?.Invoke(message);

                                    if (!message.Handled)
                                    {
                                        if (!this.unhandledMessages.Contains(message.Text))
                                            this.unhandledMessages.Add(message.Text);
                                    }
                                    else
                                        this.ParsedMessages++;
                                }

                                this.Progress?.Invoke((float)this.ParsedMessages / this.TotalMessages);
                            }

                            while (reader.EndOfStream)
                                await Task.Delay(50);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Exception?.Invoke(e);
            }
        }
    }
}
