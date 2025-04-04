using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.Privilege
{
    public interface IPrivilegeDao
    {
        public Dictionary<string, List<string>> GetUserPrivileges(string name);
        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption);
        public void RevokePrivilegesFromUser(string name, string privilege, string table_name);
        public void CreateView(string viewName, string columns, string tableName, string condition);
        public List<string> GetPrivilegesObjectType(string name, string objectType);
        public List<string> GetColumns(string objectName);
    }
}
