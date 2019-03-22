using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class UnhandledMessages : Form
    {
        private LogReader logReader;

        public UnhandledMessages(LogReader logReader)
        {
            InitializeComponent();
            this.logReader = logReader;
        }

        private void UnhandledMessages_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            e.Cancel = true;
            this.Hide();
        }

        private void UnhandledMessages_Shown(object sender, EventArgs e)
        {
            this.txtMessages.Text = String.Join(Environment.NewLine, this.logReader.UnhandledMessages);
        }
    }
}
