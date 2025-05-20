using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.MetaData.TableView
{
    class TableViewUserDao : ITableViewDao
    {
        private OracleConnection sqlConnection;
        
        public TableViewUserDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public List<OracleObject> getAllTable()
        {
            throw new NotImplementedException();
        }
        public string? GetTextOfView(string viewName)
        {
            string? result = null;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_GetTextOfView", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_view_name", OracleDbType.Varchar2).Value = viewName.ToUpper();
                OracleParameter clobParam = new OracleParameter("v_result", OracleDbType.Clob)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(clobParam);

                cmd.ExecuteNonQuery();

                if (clobParam.Value != DBNull.Value)
                {
                    OracleClob clob = (OracleClob)clobParam.Value;
                    if(!clob.IsNull)
                        result = clob.Value;
                    clob.Dispose();
                }
            }
            return result;
        }
    }
}
