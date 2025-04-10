using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.Role
{
    public interface IRoleDao
    {
        public bool CheckExist(string type, string name);
        public void CreateRole(string roleName);
        public void DropRole(string roleName);
        public List<string> GetAllRoles();
        public List<string>? GetUserRoles(string name);
        public void GrantRole(string username, string rolename, string withGrantOption);
        public void RevokeRoleFromUser(string username, string rolename);
        public List<Model.Role> GetAllRolesWithRoleClass();
    }
}
