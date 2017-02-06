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
            // TODO: Add update command functionality
            throw new NotImplementedException();
        }
    }
}
