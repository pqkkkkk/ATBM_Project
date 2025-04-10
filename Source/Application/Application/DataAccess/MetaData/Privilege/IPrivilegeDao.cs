using System.Collections.Generic;
using Application.Model;

namespace Application.DataAccess.MetaData.Privilege
{
    public interface IPrivilegeDao
    {
        public Dictionary<string, List<string>> GetUserPrivileges(string name);
        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption);
        public void RevokePrivilegesFromUser(string name, string privilege, string table_name);
        public void CreateView(string viewName, string columns, string tableName, string condition);
        public List<Model.Privilege> GetPrivilegesOfUserOnSpecificObjectType(string name, string objectType);
        public List<Model.Privilege> GetSystemPrivilegesOfUser(string name);
        public List<Model.ColumnOfObject> GetColumns(string objectName);
        public List<Application.Model.Role> GetAllRolesOfUser(string username);
        public List<Model.OracleObject> GetAllInstanceOfSpecificObject(string objectType);
    }
}
