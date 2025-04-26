using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.DataAccess.MetaData.Privilege
{
    class PrivilegeXAdminDao : IPrivilegeDao
    {
        public void CreateView(string viewName, string columns, string tableName, string condition)
        {
            throw new NotImplementedException();
        }

        public List<OracleObject> GetAllInstanceOfSpecificObject(string objectType)
        {
            throw new NotImplementedException();
        }

        public List<Model.Role> GetAllRolesOfUser(string username)
        {
            throw new NotImplementedException();
        }

        public List<Model.Privilege> GetAllSystemPrivileges()
        {
            throw new NotImplementedException();
        }

        public List<Model.Privilege> GetColumnPrivilegesOfUser(string name)
        {
            throw new NotImplementedException();
        }

        public List<ColumnOfObject> GetColumns(string objectName)
        {
            throw new NotImplementedException();
        }

        public List<Model.Privilege> GetPrivilegesOfUserOnSpecificObjectType(string name, string objectType)
        {
            throw new NotImplementedException();
        }

        public List<Model.Privilege> GetSystemPrivilegesOfUser(string name)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<string>> GetUserPrivileges(string name)
        {
            throw new NotImplementedException();
        }

        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption)
        {
            throw new NotImplementedException();
        }

        public void GrantPrivilegesOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns, string withGrantOption)
        {
            throw new NotImplementedException();
        }

        public void GrantSelectOnSpecificColumnsOfTableOrView(string name, string table_name, string columns, string withGrantOption)
        {
            throw new NotImplementedException();
        }

        public void GrantSystemPrivilegesToUser(string name, string privilege, string withAdminOption)
        {
            throw new NotImplementedException();
        }

        public void RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns)
        {
            throw new NotImplementedException();
        }

        public void RevokePrivilegesOfUserOnSpecificObjectType(string name, string privilege, string table_name)
        {
            throw new NotImplementedException();
        }

        public void RevokeSystemPrivilegesFromUser(string name, string privilege)
        {
            throw new NotImplementedException();
        }
    }
}
