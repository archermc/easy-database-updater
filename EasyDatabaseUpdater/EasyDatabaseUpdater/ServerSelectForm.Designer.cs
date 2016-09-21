namespace EasyDatabaseUpdater
{
    partial class ServerSelectForm
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
            this.serverNameLbl = new System.Windows.Forms.Label();
            this.usernameLbl = new System.Windows.Forms.Label();
            this.passwordLbl = new System.Windows.Forms.Label();
            this.serverNameTxtBox = new System.Windows.Forms.TextBox();
            this.usernameTxtBox = new System.Windows.Forms.TextBox();
            this.passwordTxtBox = new System.Windows.Forms.TextBox();
            this.integratedSecurityRdoBtn = new System.Windows.Forms.RadioButton();
            this.sqlAccountRdoBtn = new System.Windows.Forms.RadioButton();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.nextBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverNameLbl
            // 
            this.serverNameLbl.AutoSize = true;
            this.serverNameLbl.Location = new System.Drawing.Point(14, 26);
            this.serverNameLbl.Name = "serverNameLbl";
            this.serverNameLbl.Size = new System.Drawing.Size(72, 13);
            this.serverNameLbl.TabIndex = 0;
            this.serverNameLbl.Text = "Server Name:";
            // 
            // usernameLbl
            // 
            this.usernameLbl.AutoSize = true;
            this.usernameLbl.Location = new System.Drawing.Point(28, 106);
            this.usernameLbl.Name = "usernameLbl";
            this.usernameLbl.Size = new System.Drawing.Size(58, 13);
            this.usernameLbl.TabIndex = 1;
            this.usernameLbl.Text = "Username:";
            // 
            // passwordLbl
            // 
            this.passwordLbl.AutoSize = true;
            this.passwordLbl.Location = new System.Drawing.Point(30, 141);
            this.passwordLbl.Name = "passwordLbl";
            this.passwordLbl.Size = new System.Drawing.Size(56, 13);
            this.passwordLbl.TabIndex = 2;
            this.passwordLbl.Text = "Password:";
            // 
            // serverNameTxtBox
            // 
            this.serverNameTxtBox.Location = new System.Drawing.Point(92, 23);
            this.serverNameTxtBox.Name = "serverNameTxtBox";
            this.serverNameTxtBox.Size = new System.Drawing.Size(159, 20);
            this.serverNameTxtBox.TabIndex = 3;
            // 
            // usernameTxtBox
            // 
            this.usernameTxtBox.Location = new System.Drawing.Point(92, 103);
            this.usernameTxtBox.Name = "usernameTxtBox";
            this.usernameTxtBox.Size = new System.Drawing.Size(159, 20);
            this.usernameTxtBox.TabIndex = 4;
            // 
            // passwordTxtBox
            // 
            this.passwordTxtBox.Location = new System.Drawing.Point(92, 138);
            this.passwordTxtBox.Name = "passwordTxtBox";
            this.passwordTxtBox.Size = new System.Drawing.Size(159, 20);
            this.passwordTxtBox.TabIndex = 5;
            // 
            // integratedSecurityRdoBtn
            // 
            this.integratedSecurityRdoBtn.AutoSize = true;
            this.integratedSecurityRdoBtn.Checked = true;
            this.integratedSecurityRdoBtn.Location = new System.Drawing.Point(17, 67);
            this.integratedSecurityRdoBtn.Name = "integratedSecurityRdoBtn";
            this.integratedSecurityRdoBtn.Size = new System.Drawing.Size(114, 17);
            this.integratedSecurityRdoBtn.TabIndex = 6;
            this.integratedSecurityRdoBtn.TabStop = true;
            this.integratedSecurityRdoBtn.Text = "Integrated Security";
            this.integratedSecurityRdoBtn.UseVisualStyleBackColor = true;
            this.integratedSecurityRdoBtn.CheckedChanged += new System.EventHandler(this.rdoBtn_CheckedChanged);
            // 
            // sqlAccountRdoBtn
            // 
            this.sqlAccountRdoBtn.AutoSize = true;
            this.sqlAccountRdoBtn.Location = new System.Drawing.Point(162, 67);
            this.sqlAccountRdoBtn.Name = "sqlAccountRdoBtn";
            this.sqlAccountRdoBtn.Size = new System.Drawing.Size(89, 17);
            this.sqlAccountRdoBtn.TabIndex = 7;
            this.sqlAccountRdoBtn.Text = "SQL Account";
            this.sqlAccountRdoBtn.UseVisualStyleBackColor = true;
            this.sqlAccountRdoBtn.CheckedChanged += new System.EventHandler(this.rdoBtn_CheckedChanged);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(17, 180);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // nextBtn
            // 
            this.nextBtn.Location = new System.Drawing.Point(176, 180);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(75, 23);
            this.nextBtn.TabIndex = 9;
            this.nextBtn.Text = "Next";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // ServerSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 215);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.sqlAccountRdoBtn);
            this.Controls.Add(this.integratedSecurityRdoBtn);
            this.Controls.Add(this.passwordTxtBox);
            this.Controls.Add(this.usernameTxtBox);
            this.Controls.Add(this.serverNameTxtBox);
            this.Controls.Add(this.passwordLbl);
            this.Controls.Add(this.usernameLbl);
            this.Controls.Add(this.serverNameLbl);
            this.Name = "ServerSelectForm";
            this.Text = "ServerSelectForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverNameLbl;
        private System.Windows.Forms.Label usernameLbl;
        private System.Windows.Forms.Label passwordLbl;
        private System.Windows.Forms.TextBox serverNameTxtBox;
        private System.Windows.Forms.TextBox usernameTxtBox;
        private System.Windows.Forms.TextBox passwordTxtBox;
        private System.Windows.Forms.RadioButton integratedSecurityRdoBtn;
        private System.Windows.Forms.RadioButton sqlAccountRdoBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button nextBtn;
    }
}