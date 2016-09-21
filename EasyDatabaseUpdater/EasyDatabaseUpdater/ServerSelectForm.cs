using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyDatabaseUpdater
{
    public partial class ServerSelectForm : Form
    {
        public ServerSelectForm()
        {
            InitializeComponent();

            Size = new Size(289, 181);

            usernameLbl.Visible = false;
            usernameTxtBox.Visible = false;
            passwordLbl.Visible = false;
            passwordTxtBox.Visible = false;

            cancelBtn.Location = new Point(17, 103);
            nextBtn.Location = new Point(176, 103);
        }

        private void rdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (sqlAccountRdoBtn.Checked)
            {
                Size = new Size(289, 254);

                usernameLbl.Visible = true;
                usernameTxtBox.Visible = true;
                passwordLbl.Visible = true;
                passwordTxtBox.Visible = true;

                cancelBtn.Location = new Point(17, 181);
                nextBtn.Location = new Point(176, 181);
            }
            else
            {
                Size = new Size(289, 181);

                usernameLbl.Visible = false;
                usernameTxtBox.Visible = false;
                passwordLbl.Visible = false;
                passwordTxtBox.Visible = false;

                cancelBtn.Location = new Point(17, 103);
                nextBtn.Location = new Point(176, 103);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            TableSelectForm tblSelect;

            if (sqlAccountRdoBtn.Checked)
                tblSelect = new TableSelectForm(serverNameTxtBox.Text, false, usernameTxtBox.Text, passwordTxtBox.Text);
            else
                tblSelect = new TableSelectForm(serverNameTxtBox.Text);

            tblSelect.Show();
            Hide();
        }
    }
}
