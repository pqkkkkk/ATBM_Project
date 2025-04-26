using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.Role
{
    class RoleXAdminDao : IRoleDao
    {
        public bool CheckExist(string type, string name)
        {
            throw new NotImplementedException();
        }

        public bool CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool DropRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public List<Model.Role> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public List<Model.Role> GetAllRolesWithRoleClass()
        {
            throw new NotImplementedException();
        }

        public List<string>? GetUserRoles(string name)
        {
            throw new NotImplementedException();
        }

        public void GrantRole(string username, string rolename, string withGrantOption)
        {
            throw new NotImplementedException();
        }

        public void RevokeRoleFromUser(string username, string rolename)
        {
            throw new NotImplementedException();
        }
    }
}
