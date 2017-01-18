using System;
using System.Collections.Generic;
using System.Data;
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

        public string GenerateSQLCommand()
        {
            StringBuilder command = new StringBuilder("INSERT INTO " + rowToAdd.Table.TableName + " VALUES (");

            foreach (var r in rowToAdd.ItemArray)
                command.Append(r.ToString() + ", ");

            string toReturn = Regex.Replace(command.ToString(),", $", ");");

            return toReturn;
        }
    }
}
