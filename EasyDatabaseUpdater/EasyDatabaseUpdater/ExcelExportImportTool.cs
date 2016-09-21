using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EasyDatabaseUpdater
{
    public static class ExcelExportImportTool
    {
        public static bool ExportTablesToExcel(string connectionString, List<string> tableNames = null)
        {
            if (tableNames == null)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    DataTable schema = con.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                        tableNames.Add(row[2].ToString());
                }
            }

            foreach (string tableName in tableNames)
            {
                DataTable table = new DataTable();

                using (var con = new SqlConnection(connectionString))
                {
                    string command = "SELECT * FROM " + tableName;

                    using (var cmd = new SqlCommand(command, con))
                    {
                        SqlDataAdapter adapt = new SqlDataAdapter(cmd);

                        con.Open();

                        adapt.FillSchema(table, SchemaType.Source);
                        adapt.Fill(table);

                        con.Close();
                    }
                }

                Console.Read();
            }
        }
                

            return true;
        }
    }
}
