using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace EasyDatabaseUpdater
{
    public static class ExcelExportImportTool
    {
        public static bool ExportTablesToExcel(string connectionString, List<string> tableNames = null)
        {
            var datatablesToExport = new List<DataTable>();

            // populate the table names with every table in the database if the list is null
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

            // then for each table, export every one of its rows to Excel
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (string tableName in tableNames)
                {
                    DataTable table = new DataTable();

                    string command = "SELECT * FROM " + tableName;

                    using (var cmd = new SqlCommand(command, con))
                    {
                        SqlDataAdapter adapt = new SqlDataAdapter(cmd);

                        adapt.FillSchema(table, SchemaType.Source);
                        adapt.Fill(table);

                        datatablesToExport.Add(table);
                    }
                }

                con.Close();
            }

            WriteDataTablesToExcel(datatablesToExport);

            return true;
        }

        private static string WriteDataTablesToExcel(List<DataTable> tables)
        {
            // open excel application
            Application excel = new Application();

            excel.DisplayAlerts = false;

            Workbooks wbs = excel.Workbooks;
            Workbook wb = wbs.Add(XlWBATemplate.xlWBATWorksheet);
            Sheets sh = wb.Sheets;
            Worksheet ws;
            Range Cells;

            // start for each
            for (int currentTableIndex = 0; currentTableIndex < tables.Count; currentTableIndex++)
            {
                DataTable table = tables[currentTableIndex];

                // create a sheet for each table, unless there's already a default sheet in that place
                if (sh.Count >= currentTableIndex + 1)
                    ws = sh[currentTableIndex + 1];
                else
                    ws = sh.Add(After: wb.Sheets[wb.Sheets.Count]);

                ws.Name = table.TableName;
                Cells = ws.Cells;


                // write the column names on the first row
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    ws.Cells[1, i + 1].Value = table.Columns[i].ColumnName;
                }

                // write the row data for each row
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    object[] row = table.Rows[j].ItemArray;

                    for (int l = 0; l < row.Length; l++)
                        ws.Cells[j + 2, l + 1].Value = row[l];
                }
            }

            excel.Visible = true;
            // open file dialog asking where to save file


            // close out all those marshall interops ughghghgh

            return ""; //filepath
        }
    }
}
