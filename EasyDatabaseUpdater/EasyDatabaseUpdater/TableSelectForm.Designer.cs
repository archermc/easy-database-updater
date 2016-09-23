namespace EasyDatabaseUpdater
{
    partial class TableSelectForm
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
            this.databaseSelectorCmbBox = new System.Windows.Forms.ComboBox();
            this.tableNameLstBox = new System.Windows.Forms.CheckedListBox();
            this.exportTablesBtn = new System.Windows.Forms.Button();
            this.importTablesBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // databaseSelectorCmbBox
            // 
            this.databaseSelectorCmbBox.FormattingEnabled = true;
            this.databaseSelectorCmbBox.Location = new System.Drawing.Point(12, 12);
            this.databaseSelectorCmbBox.Name = "databaseSelectorCmbBox";
            this.databaseSelectorCmbBox.Size = new System.Drawing.Size(121, 21);
            this.databaseSelectorCmbBox.TabIndex = 0;
            this.databaseSelectorCmbBox.Text = "Choose Database";
            this.databaseSelectorCmbBox.SelectedIndexChanged += new System.EventHandler(this.databaseSelectorCmbBox_SelectedIndexChanged);
            // 
            // tableNameLstBox
            // 
            this.tableNameLstBox.FormattingEnabled = true;
            this.tableNameLstBox.Location = new System.Drawing.Point(12, 39);
            this.tableNameLstBox.Name = "tableNameLstBox";
            this.tableNameLstBox.Size = new System.Drawing.Size(154, 244);
            this.tableNameLstBox.TabIndex = 1;
            // 
            // exportTablesBtn
            // 
            this.exportTablesBtn.Location = new System.Drawing.Point(231, 261);
            this.exportTablesBtn.Name = "exportTablesBtn";
            this.exportTablesBtn.Size = new System.Drawing.Size(86, 23);
            this.exportTablesBtn.TabIndex = 2;
            this.exportTablesBtn.Text = "Export Tables";
            this.exportTablesBtn.UseVisualStyleBackColor = true;
            this.exportTablesBtn.Click += new System.EventHandler(this.exportTablesBtn_Click);
            // 
            // importTablesBtn
            // 
            this.importTablesBtn.Location = new System.Drawing.Point(231, 231);
            this.importTablesBtn.Name = "importTablesBtn";
            this.importTablesBtn.Size = new System.Drawing.Size(86, 23);
            this.importTablesBtn.TabIndex = 3;
            this.importTablesBtn.Text = "Import Tables";
            this.importTablesBtn.UseVisualStyleBackColor = true;
            // 
            // TableSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 296);
            this.Controls.Add(this.importTablesBtn);
            this.Controls.Add(this.exportTablesBtn);
            this.Controls.Add(this.tableNameLstBox);
            this.Controls.Add(this.databaseSelectorCmbBox);
            this.Name = "TableSelectForm";
            this.Text = "Easy Database Updater";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TableSelectForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox databaseSelectorCmbBox;
        private System.Windows.Forms.CheckedListBox tableNameLstBox;
        private System.Windows.Forms.Button exportTablesBtn;
        private System.Windows.Forms.Button importTablesBtn;
    }
}

