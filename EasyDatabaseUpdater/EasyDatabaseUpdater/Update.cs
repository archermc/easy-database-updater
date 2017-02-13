using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDatabaseUpdater
{
    class Update : IModification
    {
        DataRow originalRow;
        DataRow modifiedRow;

        public Update(DataRow oRow, DataRow mRow)
        {
            originalRow = oRow;
            modifiedRow = mRow;
        }

        public SqlCommand GenerateSQLCommand(SqlConnection con)
        {
            StringBuilder commandStr = new StringBuilder("UPDATE " + originalRow.Table.TableName + " SET ");
            // TODO: FINISH UPDATE
            SqlCommand command = new SqlCommand();

            for (int i = 0; i < originalRow.ItemArray.Length; i++)
            {
                if (!originalRow.Table.Columns[i].AutoIncrement &&
                    !originalRow.ItemArray[i].Equals(modifiedRow.ItemArray[i]))
                {
                    commandStr.Append("")
                }
            }
            // TODO: Add update command functionality
            throw new NotImplementedException();
        }
    }
}
