using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        public string GenerateSQLCommand()
        {
            
            // TODO: Add add command functionality
            throw new NotImplementedException();


        }
    }
}
