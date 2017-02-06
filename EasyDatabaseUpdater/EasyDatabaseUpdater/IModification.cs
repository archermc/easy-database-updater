using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDatabaseUpdater
{
    public interface IModification
    {
        SqlCommand GenerateSQLCommand(SqlConnection con);
    }
}
