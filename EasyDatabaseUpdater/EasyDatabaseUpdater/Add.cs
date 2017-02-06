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
    class Add : IModification
    {
        DataRow rowToAdd;

        public Add(DataRow toAdd)
        {
            rowToAdd = toAdd;
        }

        public SqlCommand GenerateSQLCommand(SqlConnection con)
        {
            StringBuilder commandStr = new StringBuilder("INSERT INTO " + rowToAdd.Table.TableName + " (");
            StringBuilder valuesStr = new StringBuilder("VALUES (");
            SqlCommand command = new SqlCommand();

            //for (int i = 0; i < rowToAdd.ItemArray.Length; i++)
            //{
            //    if (rowToAdd.Table.Columns[i].AutoIncrement)
            //    {
            //        command.Parameters.AddWithValue("@paramd", "DEFAULT");
            //        commandStr.Append("@paramd, ");
            //    }

            //    command.Parameters.AddWithValue("@param" + i, rowToAdd.ItemArray[i]);
            //    commandStr.Append("@param" + i + ", ");
            //}

            for (int i = 0; i < rowToAdd.ItemArray.Length; i++)
            {
                if (!rowToAdd.Table.Columns[i].AutoIncrement)
                {
                    command.Parameters.AddWithValue("@param" + i, rowToAdd.ItemArray[i]);
                    commandStr.Append(rowToAdd.Table.Columns[i].ColumnName + ", ");
                    valuesStr.Append("@param" + i + ", ");
                }
            }

            command.CommandText = Regex.Replace(commandStr.ToString(), ", $", ") ") + Regex.Replace(valuesStr.ToString(), ", $", ");");
            command.Connection = con;

            return command;
        }
    }
}
