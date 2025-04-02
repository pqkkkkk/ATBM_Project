using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Windows.Media.Audio;

namespace Application.DataAccess.MetaData
{
    public interface IMetaDataDao {
        public bool CheckExist(string type, string name);
        public void CreateUser(string username, string password);
        public void DeleteUser(string username);
        public void UpdatePassword(string username, string password);
        public void CreateRole(string roleName);
        public void DropRole(string roleName);
        public List<string> GetAllRoles();
        public List<string>? GetUserRoles(string name);
        public void GrantRole(string username, string rolename, string withGrantOption);
        public void RevokeRoleFromUser(string username, string rolename);
        public Dictionary<string, List<string>> GetUserPrivileges(string name);
        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption);
        public void RevokePrivilegesFromUser(string name, string privilege, string table_name);
        public void CreateView(string viewName, string columns, string tableName, string condition);
        public List<string> GetPrivilegesObjectType(string name, string objectType);
        public List<string> GetColumns(string objectName);
    }
}
