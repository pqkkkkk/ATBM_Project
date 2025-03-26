using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.MetaData
{
    class MetaDataOracleDao : IMetaDataDao
    {
        private OracleConnection sqlConnection;

        public MetaDataOracleDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public List<string> GetUserRoles()
        {
            throw new NotImplementedException();
        }
        public List<UserPrivilege> GetUserPrivileges()
        {
            throw new NotImplementedException();
        }
    }
}
