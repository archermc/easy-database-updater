using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EasyDatabaseUpdater
{
    public class ExcelExportImportTool : IDisposable
    {
        private string _connectionString;
        private List<DataTable> _originalTables;

        public ExcelExportImportTool(string connectionString)
        {
            _connectionString = connectionString;
            _originalTables = null;
        }

        public bool ExportTablesToExcel(List<string> tableNames = null)
        {
            var datatablesToExport = new List<DataTable>();

            // populate the table names with every table in the database if the list is null
            if (tableNames == null)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    DataTable schema = con.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                        tableNames.Add(row[2].ToString());
                }
            }

            // then for each table, export every one of its rows to Excel
            using (var con = new SqlConnection(_connectionString))
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

        private string WriteDataTablesToExcel(List<DataTable> tables)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "xlsx";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFile.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";

            // open excel application
            Application excel = new Application();

            excel.DisplayAlerts = false;

            Workbooks wbs = excel.Workbooks;
            Workbook wb = wbs.Add(XlWBATemplate.xlWBATWorksheet);
            Sheets sh = wb.Sheets;
            Worksheet ws = null;
            Range cells = null;

            Range headerRange = null;
            Interior headerInterior = null;
            Font headerFont = null;
            Range usedRange = null;
            Range rows = null;
            Range cols = null;

            Range pkColumn = null;
            Interior pkInterior = null;

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
                cells = ws.Cells;
                cells.NumberFormat = "@";

                // write the column names on the first row
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    cells[1, i + 1].Value = table.Columns[i].ColumnName;
                }

                // write the row data for each row
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    object[] row = table.Rows[j].ItemArray;

                    for (int l = 0; l < row.Length; l++)
                    { 
                        if (table.Columns[l].DataType == typeof(DateTime))
                            cells[j + 2, l + 1].Value = ((DateTime)row[l]).ToString("MM-dd-yyyy hh:mm:ss");
                        else
                            cells[j + 2, l + 1].Value = row[l];
                    }
                }

                #region FORMATTING
                headerRange = cells[1].EntireRow;
                headerInterior = headerRange.Interior;
                headerFont = headerRange.Font;

                headerInterior.Color = System.Drawing.Color.DarkGray;
                headerFont.Bold = true;

                usedRange = ws.UsedRange;
                usedRange.ColumnWidth = 20;
                rows = usedRange.Rows;
                cols = usedRange.Columns;
                
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    if (GetPrimaryKeys(table)[col])
                    {
                        pkColumn = ws.Range[usedRange[2,col + 1], usedRange[rows.Count,col+1]];
                        pkInterior = pkColumn.Interior;
                        pkInterior.Color = System.Drawing.Color.LightYellow;
                    }
                }
                #endregion
            }

            // open file dialog asking where to save file and save file there
            if (DialogResult.OK == saveFile.ShowDialog())
                wb.SaveAs(saveFile.FileName);

            excel.DisplayAlerts = true;

            // close out all those marshall interops ughghghgh
            wb.Close();
            excel.Quit();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.FinalReleaseComObject(pkInterior);
            Marshal.FinalReleaseComObject(pkColumn);
            Marshal.FinalReleaseComObject(cols);
            Marshal.FinalReleaseComObject(rows);
            Marshal.FinalReleaseComObject(usedRange);
            Marshal.FinalReleaseComObject(headerFont);
            Marshal.FinalReleaseComObject(headerInterior);
            Marshal.FinalReleaseComObject(headerRange);

            Marshal.FinalReleaseComObject(cells);
            Marshal.FinalReleaseComObject(ws);
            Marshal.FinalReleaseComObject(sh);
            Marshal.FinalReleaseComObject(wb);
            Marshal.FinalReleaseComObject(wbs);
            Marshal.FinalReleaseComObject(excel);

            return saveFile.FileName; //filepath
        }

        public List<DataTable> ImportTablesFromExcel()
        {
            List<DataTable> importedTables;
            List<string> tableNames = new List<string>();
            string filePath = "";

            // file browser where one chooses the excel file to use
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "xlsx";
            openFile.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";

            if (DialogResult.OK == openFile.ShowDialog())
                filePath = openFile.FileName;

            Application excel = new Application();
            Workbooks wbs = excel.Workbooks;
            Workbook wb = wbs.Open(filePath);
            Sheets sh = wb.Sheets;

            // take every sheet name and get the original table, storing in the object's memory and getting clones of the tables
            for (int shI = 1; shI <= sh.Count; shI++)
                tableNames.Add(sh[shI].Name);

            importedTables = GetDataTableClones(tableNames);

            Worksheet ws = null;
            Range usedRange = null;
            Range rows = null;
            object[,] cells = null;

            // for loop iterating through each sheet
            for (int sheetInd = 0; sheetInd < sh.Count; sheetInd++)
            {
                DataTable currentTable = importedTables[sheetInd];
                // get the schema from the original table using the Sheet name
                ws = sh[currentTable.TableName];
                usedRange = ws.UsedRange;
                rows = usedRange.Rows;
                cells = usedRange.Value;

                int rowCount = rows.Count;

                // grab every row and stick it in the datatable
                for (int r = 2; r <= rowCount; r++)
                {
                    DataRow row = currentTable.NewRow();

                    for (int c = 0; c < row.ItemArray.Length; c++)
                        row[c] = cells[r, c + 1];

                    currentTable.Rows.Add(row);
                }
            }

            return importedTables;
        }

        private List<DataTable> GetDataTableClones(List<string> tableNames)
        {
            _originalTables = new List<DataTable>();
            var tableClones = new List<DataTable>();

            using (var con = new SqlConnection(_connectionString))
            {
                foreach (string name in tableNames)
                {
                    using (var com = new SqlCommand("SELECT * FROM " + name,con))
                    {
                        DataTable table = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(com);

                        da.FillSchema(table,SchemaType.Source);
                        da.Fill(table);

                        _originalTables.Add(table);
                        tableClones.Add(table.Clone());
                    }
                }
            }

            return tableClones;
        }

        /// <summary>
        /// Creates a boolean array of trues and falses based on whether that index of DataTable.Columns is a primary key.
        /// </summary>
        /// <param name="table">DataTable to find the primary keys of.</param>
        /// <returns>A boolean array with cooresponding "true" values at the indices of the primary keys.</returns>
        private static bool[] GetPrimaryKeys(DataTable table)
        {
            return table.Columns.OfType<DataColumn>().ToList().Select(s => table.PrimaryKey.Contains(s)).ToArray();
        }

        public void Dispose()
        {
            
        }
    }
}
