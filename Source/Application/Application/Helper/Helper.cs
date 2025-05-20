using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    class Helper
    {
        public Helper()
        {
        }
        public string GetTableNameFromTextOfView(string text)
        {
            int spaceIdx1 = text.IndexOf("FROM") + 5;
            int spaceIdx2 = text.IndexOf(" ", spaceIdx1 + 1);
            if (spaceIdx2 == -1)
                spaceIdx2 = text.Length;

            string tableName = text.Substring(spaceIdx1, spaceIdx2 - spaceIdx1).Trim();

            return tableName;
        }
    }
}
