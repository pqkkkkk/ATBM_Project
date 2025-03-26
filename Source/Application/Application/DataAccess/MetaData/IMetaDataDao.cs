using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.DataAccess.MetaData
{
    public interface IMetaDataDao {
        public List<string>? GetUserRoles();
        public List<UserPrivilege> GetUserPrivileges();
    }
}
