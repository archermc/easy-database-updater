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
            Size = new Size(381, 200);
        }

        private void rdoBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (sqlAccountRdoBtn.Checked)
                Size = new Size(381, 300);
            else
                Size = new Size(381, 200);
        }
    }
}
