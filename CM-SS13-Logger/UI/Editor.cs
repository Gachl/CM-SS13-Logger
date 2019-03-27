using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CM_SS13_Logger
{
    public partial class Editor<T> : Form
    {
        private List<T> data;

        public Type Type => typeof(T);
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

        private void Editor_Shown(object sender, EventArgs e)
        {
            this.dgvEditor.AutoResizeColumns();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            BindingSource bindingSource = (BindingSource)this.dgvEditor.DataSource;
            List<int> selectedRows = new List<int>();

            // Item moving
            foreach (DataGridViewRow row in this.dgvEditor.SelectedCells.Cast<DataGridViewCell>().GroupBy(r => r.RowIndex).Select(r => r.First().OwningRow))
            {
                int index = row.Index;

                if (index <= 0 || index >= bindingSource.Count)
                    continue;

                // Move item
                T item = (T)bindingSource[index];
                ((List<T>)bindingSource.DataSource).Remove(item);

                index--;

                ((List<T>)bindingSource.DataSource).Insert(index, item);
                selectedRows.Add(index);
            }

            if (selectedRows.Count == 0)
                return;

            bindingSource.ResetBindings(false);
            this.dgvEditor.ClearSelection();

            // Re-selection
            foreach (int selectRow in selectedRows)
                this.dgvEditor.Rows[selectRow].Selected = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            BindingSource bindingSource = (BindingSource)this.dgvEditor.DataSource;
            List<int> selectedRows = new List<int>();

            // Item moving
            foreach (DataGridViewRow row in this.dgvEditor.SelectedCells.Cast<DataGridViewCell>().GroupBy(r => r.RowIndex).Select(r => r.First().OwningRow))
            {
                int index = row.Index;

                if (index >= bindingSource.Count - 1)
                    continue;

                // Move item
                T item = (T)bindingSource[index];
                ((List<T>)bindingSource.DataSource).Remove(item);

                index++;

                ((List<T>)bindingSource.DataSource).Insert(index, item);
                selectedRows.Add(index);
            }

            if (selectedRows.Count == 0)
                return;

            bindingSource.ResetBindings(false);
            this.dgvEditor.ClearSelection();

            // Re-selection
            foreach (int selectRow in selectedRows)
                this.dgvEditor.Rows[selectRow].Selected = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgvEditor.SelectedCells.Cast<DataGridViewCell>().GroupBy(r => r.RowIndex).Select(r => r.First().OwningRow))
                if (!row.IsNewRow)
                    this.dgvEditor.Rows.Remove(row);
        }
    }
}
