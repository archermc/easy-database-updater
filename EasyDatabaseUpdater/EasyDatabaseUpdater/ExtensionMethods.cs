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
    }
}
