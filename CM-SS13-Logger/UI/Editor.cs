using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class Editor<T> : Form
    {
        private List<T> data;

        public List<T> Data => this.data.ToList();

        public Editor(List<T> data)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            this.data = data.ToList();
            this.dgvEditor.DataSource = new BindingSource()
            {
                DataSource = this.data
            };
        }

        private void brnOK_Click(object sender, EventArgs e)
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
