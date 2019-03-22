namespace CM_SS13_Logger
{
    partial class Editor<T>
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvEditor = new System.Windows.Forms.DataGridView();
            this.brnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEditor
            // 
            this.dgvEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEditor.Location = new System.Drawing.Point(12, 12);
            this.dgvEditor.Name = "dgvEditor";
            this.dgvEditor.Size = new System.Drawing.Size(694, 314);
            this.dgvEditor.TabIndex = 0;
            // 
            // brnOK
            // 
            this.brnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.brnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.brnOK.Location = new System.Drawing.Point(631, 332);
            this.brnOK.Name = "brnOK";
            this.brnOK.Size = new System.Drawing.Size(75, 23);
            this.brnOK.TabIndex = 1;
            this.brnOK.Text = "OK";
            this.brnOK.UseVisualStyleBackColor = true;
            this.brnOK.Click += new System.EventHandler(this.brnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(550, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Editor
            // 
            this.AcceptButton = this.brnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(718, 363);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.brnOK);
            this.Controls.Add(this.dgvEditor);
            this.Name = "Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEditor;
        private System.Windows.Forms.Button brnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}