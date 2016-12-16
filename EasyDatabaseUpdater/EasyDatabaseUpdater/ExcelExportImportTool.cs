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
using System.Globalization;
using System.Threading;

namespace EasyDatabaseUpdater
{
    public class ExcelExportImportTool : IDisposable
    {
        private string _connectionString;
        private string _dtFormat;
        private List<DataTable> _originalTables;

        public ExcelExportImportTool(string connectionString)
        {
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;

            _connectionString = connectionString;
            _dtFormat = "MM/dd/yyyy HH:mm:ss";
            _originalTables = null;
        }

        /// <summary>
        /// Takes a list of table names (or all the table names if no table names are provided) and extracts them from
        /// the database in the connection string and exports them from the database to Excel
        /// </summary>
        /// <param name="tableNames">The names of tables contained in the database that the connection string
        /// refers to that we want to extract to Excel</param>
        /// <returns></returns>
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

                    // each table name is in a specific row in the database schema
                    foreach (DataRow row in schema.Rows)
                        tableNames.Add(row[2].ToString());
                }
            }

            // then for each table, export every one of its rows to Excel
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                // take the tablename and use a SELECT statement to get the rows
                foreach (string tableName in tableNames)
                {
                    DataTable table = new DataTable();

                    string command = "SELECT * FROM " + tableName;

                    // after building the command we use the SqlDataAdapter to fill the schema and actual rows of the DataTable
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


        /// <summary>
        /// Writes each table selected to Excel using the interop
        /// </summary>
        /// <param name="tables">Tables to write to the chosen Excel file</param>
        /// <returns>String representation of the file location the tables were saved at.</returns>
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

            // for each table add all of its rows to Excel
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
                            cells[j + 2, l + 1].Value = ((DateTime)row[l]).ToString(_dtFormat); //"MM/dd/yyyy hh:mm:ss");
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

        /// <summary>
        /// Imports a set of Tables from Excel into a Datatable List
        /// </summary>
        /// <returns>DataTable list that contains each table imported from Excel file</returns>
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
                    {
                        if (_originalTables[sheetInd].Columns[c].DataType == typeof(DateTime))
                        {
                            DateTime dt = new DateTime();
                            bool success = DateTime.TryParseExact(cells[r, c + 1].ToString(), _dtFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                            if (success)
                                row[c] = dt;
                            else
                                throw new Exception();
                        }
                        else
                            row[c] = cells[r, c + 1];
                    }

                    currentTable.Rows.Add(row);
                }
            }

            wb.Close();
            excel.Quit();

            //Marshal.FinalReleaseComObject(cells);
            Marshal.FinalReleaseComObject(ws);
            Marshal.FinalReleaseComObject(sh);
            Marshal.FinalReleaseComObject(wb);
            Marshal.FinalReleaseComObject(wbs);
            Marshal.FinalReleaseComObject(excel);

            return importedTables;
        }

        /// <summary>
        /// Compares the two tables and attempts to tell whether the modified table has changed, adding it to the list of IModifications
        /// that include either an add, delete, or update
        /// </summary>
        /// <param name="modifiedTable">the table that was modified in Excel to be compared to the original table</param>
        /// <returns>List of modifications to write to the table</returns>
        public List<IModification> FindTableDifferences(List<DataTable> modifiedTables)
        {
            // TODO: finish table differences
            foreach (DataTable modifiedTable in modifiedTables)
            {
                // get the original table to compare against the modified table
                DataTable originalTable = _originalTables.Where(c => _originalTables.Any(l => c.TableName == l.TableName)).ToArray()[0];


            }


            return new List<IModification>();
        }

        /// <summary>
        /// Clones the tables in the database and adds them to the original tables List.
        /// </summary>
        /// <param name="tableNames">The names of all the tables in the database you need to clone</param>
        /// <returns>List of tables that were cloned</returns>
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
