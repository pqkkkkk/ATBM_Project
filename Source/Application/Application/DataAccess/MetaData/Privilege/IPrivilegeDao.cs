using System.Collections.Generic;
using Application.Model;

namespace Application.DataAccess.MetaData.Privilege
{
    public interface IPrivilegeDao
    {
        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption);
        public void GrantPrivilegesOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns, string withGrantOption);
        public void GrantSelectOnSpecificColumnsOfTableOrView(string name, string table_name, string columns, string withGrantOption);
        public void GrantSystemPrivilegesToUser(string name, string privilege, string withAdminOption);

        public void RevokePrivilegesOfUserOnSpecificObjectType(string name, string privilege, string table_name);
        public void RevokeSystemPrivilegesFromUser(string name, string privilege);
        public void RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns);

        public void CreateView(string viewName, string columns, string tableName, string condition);

        public List<Model.Privilege> GetPrivilegesOfUserOnSpecificObjectType(string name, string objectType);
        public List<Model.Privilege> GetAllSystemPrivileges();
        public List<Model.Privilege> GetSystemPrivilegesOfUser(string name);
        public List<Model.Privilege> GetColumnPrivilegesOfUser(string name);
        public List<Model.ColumnOfObject> GetColumns(string objectName);
        public List<Application.Model.Role> GetAllRolesOfUser(string username);
        public List<Model.OracleObject> GetAllInstanceOfSpecificObject(string objectType);
        public Dictionary<string, List<string>> GetUserPrivileges(string name);
    }
}
