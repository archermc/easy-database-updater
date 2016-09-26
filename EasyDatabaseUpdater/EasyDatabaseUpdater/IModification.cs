using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDatabaseUpdater
{
    public interface IModification
    {
        string GenerateSQLCommand();

    }
}
