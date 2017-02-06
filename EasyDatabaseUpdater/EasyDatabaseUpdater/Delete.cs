using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyDatabaseUpdater
{
    class Delete : IModification
    {
        DataRow rowToDelete;

        public Delete(DataRow toDelete)
        {
            rowToDelete = toDelete;
        }

        public SqlCommand GenerateSQLCommand(SqlConnection con)
        {
            // TODO: Add delete command generation
            StringBuilder command = new StringBuilder
                ("DELETE FROM " + rowToDelete.Table.TableName + " WHERE ");

            bool[] pKeys = rowToDelete.Table.GetPrimaryKeys();

            for (int i = 0; i < pKeys.Length; i++)
            {
                if (pKeys[i])
                    command.Append(rowToDelete.Table.Columns[i].ColumnName + " = " + rowToDelete.ItemArray[i] + " AND ");
            }

            string toReturn = Regex.Replace(command.ToString(), " AND $", ";");

            return new SqlCommand(); 
        }
    }
}
