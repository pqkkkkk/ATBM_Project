﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.DataAccess.MetaData.Role
{
    public interface IRoleDao
    {
        public bool CheckExist(string role);
        public bool CreateRole(string roleName);
        public bool DropRole(string roleName);
        public List<Model.Role> GetAllRoles();
        public List<string>? GetUserRoles(string name);
        public void GrantRole(string username, string rolename, string withGrantOption);
        public void RevokeRoleFromUser(string username, string rolename);
        public List<Model.Role> GetAllRolesWithRoleClass();
    }
}
