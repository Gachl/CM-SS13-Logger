using System;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class WindowTitlePrompt : Form
    {
        public string WindowTitle => this.txtWindowTitle.Text;

        public WindowTitlePrompt(string title)
        {
            InitializeComponent();
            this.txtWindowTitle.Text = title;
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
