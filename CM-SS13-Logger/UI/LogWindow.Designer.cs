namespace CM_SS13_Logger
{
    partial class LogWindow
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
            this.components = new System.ComponentModel.Container();
            this.dgvLogMessages = new System.Windows.Forms.DataGridView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editParseRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editHighlightRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogMessages)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLogMessages
            // 
            this.dgvLogMessages.AllowUserToAddRows = false;
            this.dgvLogMessages.AllowUserToDeleteRows = false;
            this.dgvLogMessages.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLogMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogMessages.ContextMenuStrip = this.contextMenu;
            this.dgvLogMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogMessages.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLogMessages.Location = new System.Drawing.Point(0, 0);
            this.dgvLogMessages.Name = "dgvLogMessages";
            this.dgvLogMessages.RowHeadersVisible = false;
            this.dgvLogMessages.ShowCellErrors = false;
            this.dgvLogMessages.ShowEditingIcon = false;
            this.dgvLogMessages.ShowRowErrors = false;
            this.dgvLogMessages.Size = new System.Drawing.Size(800, 450);
            this.dgvLogMessages.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameWindowToolStripMenuItem,
            this.editColumnsToolStripMenuItem,
            this.editParseRulesToolStripMenuItem,
            this.editHighlightRulesToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(245, 92);
            // 
            // renameWindowToolStripMenuItem
            // 
            this.renameWindowToolStripMenuItem.Name = "renameWindowToolStripMenuItem";
            this.renameWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.renameWindowToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.renameWindowToolStripMenuItem.Text = "&Rename window";
            this.renameWindowToolStripMenuItem.Click += new System.EventHandler(this.renameWindowToolStripMenuItem_Click);
            // 
            // editColumnsToolStripMenuItem
            // 
            this.editColumnsToolStripMenuItem.Name = "editColumnsToolStripMenuItem";
            this.editColumnsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.editColumnsToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.editColumnsToolStripMenuItem.Text = "Edit &Columns";
            this.editColumnsToolStripMenuItem.Click += new System.EventHandler(this.editColumnsToolStripMenuItem_Click);
            // 
            // editParseRulesToolStripMenuItem
            // 
            this.editParseRulesToolStripMenuItem.Name = "editParseRulesToolStripMenuItem";
            this.editParseRulesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.P)));
            this.editParseRulesToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.editParseRulesToolStripMenuItem.Text = "Edit &Parse Rules";
            this.editParseRulesToolStripMenuItem.Click += new System.EventHandler(this.editParseRulesToolStripMenuItem_Click);
            // 
            // editHighlightRulesToolStripMenuItem
            // 
            this.editHighlightRulesToolStripMenuItem.Name = "editHighlightRulesToolStripMenuItem";
            this.editHighlightRulesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.H)));
            this.editHighlightRulesToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.editHighlightRulesToolStripMenuItem.Text = "Edit &Highlight Rules";
            this.editHighlightRulesToolStripMenuItem.Click += new System.EventHandler(this.editHighlightRulesToolStripMenuItem_Click);
            // 
            // LogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvLogMessages);
            this.Name = "LogWindow";
            this.ShowIcon = false;
            this.Text = "CustomLogWindow";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogMessages)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLogMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem editColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editParseRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editHighlightRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameWindowToolStripMenuItem;
    }
}