using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDatabaseUpdater
{
    static class ExtensionMethods
    {
        public static bool RowEquals(this DataRow row1, DataRow row2)
        {
            for (int i = 0; i < row1.ItemArray.Length; i++)
            {
                if (!row1.ItemArray[i].Equals(row2.ItemArray[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a boolean array of trues and falses based on whether that index of DataTable.Columns is a primary key.
        /// </summary>
        /// <param name="table">DataTable to find the primary keys of.</param>
        /// <returns>A boolean array with cooresponding "true" values at the indices of the primary keys.</returns>
        public static bool[] GetPrimaryKeys(this DataTable table)
        {
            return table.Columns
                .OfType<DataColumn>()
                .ToList()
                .Select(s => table.PrimaryKey.Contains(s))
                .ToArray();
        }
    }
}
