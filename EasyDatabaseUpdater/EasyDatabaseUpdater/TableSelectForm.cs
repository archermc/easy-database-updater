using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyDatabaseUpdater
{
    public partial class TableSelectForm : Form
    {
        private string _serverName;
        private bool _integratedSecurity;
        private string _username;
        private string _password;

        private string _connectionString;
        
        public TableSelectForm(string serverName, bool integratedSecurity = true, string username = null, string password = null)
        {
            InitializeComponent();

            _serverName = serverName;
            _integratedSecurity = integratedSecurity;
            _username = username;
            _password = password;

            _connectionString =
                "Server=" + serverName + ";" +
                "Integrated Security=" + integratedSecurity + ";" +
                (!integratedSecurity ? "User ID=" + username + "; Password=" + password + ";":"");

            using (var con = new SqlConnection(_connectionString))
            {
                List<string> defaultDatabases = new List<string>{ "master", "tempdb", "model", "msdb" };

                con.Open();
                DataTable databases = con.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    string databaseName = database.Field<string>("database_name");

                    if (!defaultDatabases.Contains(databaseName))
                        databaseSelectorCmbBox.Items.Add(databaseName);
                }
            }
        }

        private void databaseSelectorCmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(_connectionString + "initial catalog=" + databaseSelectorCmbBox.SelectedItem + ";"))
            {
                con.Open();
                DataTable schema = con.GetSchema("Tables");

                foreach (DataRow row in schema.Rows)
                    tableNameLstBox.Items.Add(row[2].ToString());
            }
        }

        private void exportTablesBtn_Click(object sender, EventArgs e)
        {
            List<string> tableNames = new List<string>();

            foreach (var item in tableNameLstBox.CheckedItems)
                tableNames.Add(item.ToString());

            using (var excelTool = new ExcelExportImportTool(_connectionString + "initial catalog=" + databaseSelectorCmbBox.SelectedItem))
                excelTool.ExportTablesToExcel(tableNames);

            MessageBox.Show("Tables successfully exported!");
        }

        private void TableSelectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void importTablesBtn_Click(object sender, EventArgs e)
        {
            using (var excelTool = new ExcelExportImportTool(_connectionString + "initial catalog=" + databaseSelectorCmbBox.SelectedItem))
            {
                excelTool.ImportTablesFromExcel();
                excelTool.
            }
        }
    }
}
