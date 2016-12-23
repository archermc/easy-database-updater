using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        string IModification.GenerateSQLCommand()
        {
            // TODO: Add delete command generation
            throw new NotImplementedException();
        }
    }
}
