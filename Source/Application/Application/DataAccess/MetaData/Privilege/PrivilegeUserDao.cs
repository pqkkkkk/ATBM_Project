using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Application.Exception;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Application.DataAccess.MetaData.Privilege
{
    class PrivilegeUserDao : IPrivilegeDao
    {
        private OracleConnection sqlConnection;
        public PrivilegeUserDao(OracleConnection sqlConnection) {
            this.sqlConnection = sqlConnection;
        }
        public void CreateView(string viewName, string columns, string tableName, string condition)
        {
            throw new NoPrivilegeException();
        }

        public List<OracleObject> GetAllInstanceOfSpecificObject(string objectType)
        {
            throw new NoPrivilegeException();
        }

        public List<Model.Role> GetAllRolesOfUser(string username)
        {
            List<Model.Role> result = new List<Model.Role>();
            return result;
        }

        public List<Model.Privilege> GetAllSystemPrivileges()
        {
            throw new NoPrivilegeException();
        }

        public List<Model.Privilege> GetColumnPrivilegesOfUser(string name)
        {
            List<Model.Privilege> result = new List<Model.Privilege>();

            return result;
        }

        public List<ColumnOfObject> GetColumns(string objectName)
        {
            throw new NoPrivilegeException();
        }

        public List<Model.Privilege> GetPrivilegesOfUserOnSpecificObjectType(string name, string objectType)
        {
            List<Model.Privilege> privileges = new List<Model.Privilege>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                string owner = "X_ADMIN";
                string sql = "SELECT * FROM ROLE_TAB_PRIVS WHERE OWNER = :owner";

                using (OracleCommand cmd = new OracleCommand(sql, sqlConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Privilege privilege = new Model.Privilege()
                        {
                            grantee = reader["ROLE"].ToString(),
                            owner = reader["OWNER"].ToString(),
                            tableName = reader["TABLE_NAME"].ToString(),
                            columnName = reader["COLUMN_NAME"].ToString(),
                            privilege = reader["PRIVILEGE"].ToString(),
                        };
                        if (privilege.grantee != null && privilege.grantee.Contains("X_"))
                        {
                            privilege.grantee = privilege.grantee.Substring(2);
                        }
                        if (privilege.grantee != null && privilege.grantee.Contains("XR_"))
                        {
                            privilege.grantee = privilege.grantee.Substring(3);
                        }
                        else
                        {
                            privilege.grantee = privilege.grantee.ToUpper();
                        }
                        privileges.Add(privilege);
                    }
                    reader.Close();
                }
                return privileges;
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
            //List<Model.Privilege> privileges = new List<Model.Privilege>();
            //try
            //{
            //    if (sqlConnection.State == ConnectionState.Closed)
            //        sqlConnection.Open();

            //    using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_GetPrivilegesOfUserOnSpecificObjectTypeThrouthRole", sqlConnection))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("p_object_type", OracleDbType.Varchar2).Value = objectType;
            //        cmd.Parameters.Add("p_user_role", OracleDbType.Varchar2).Value = name;
            //        cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            //        OracleDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            Model.Privilege privilege = new Model.Privilege()
            //            {
            //                grantee = reader["ROLE"].ToString(),
            //                owner = reader["OWNER"].ToString(),
            //                tableName = reader["TABLE_NAME"].ToString(),
            //                columnName = reader["COLUMN_NAME"].ToString(),
            //                privilege = reader["PRIVILEGE"].ToString(),
            //            };
            //            if (privilege.grantee != null && privilege.grantee.Contains("X_"))
            //            {
            //                privilege.grantee = privilege.grantee.Substring(2);
            //            }
            //            if (privilege.grantee != null && privilege.grantee.Contains("XR_"))
            //            {
            //                privilege.grantee = privilege.grantee.Substring(3);
            //            }
            //            else
            //            {
            //                privilege.grantee = privilege.grantee.ToUpper();
            //            }
            //            privileges.Add(privilege);
            //        }
            //        reader.Close();
            //        return privileges;
            //    }
            //}
            //catch (System.Exception e)
            //{
            //    throw new System.Exception(e.Message);
            //}
        }

        public List<Model.Privilege> GetSystemPrivilegesOfUser(string name)
        {
            List<Model.Privilege> result = new List<Model.Privilege>();

            return result;
        }

        public Dictionary<string, List<string>> GetUserPrivileges(string name)
        {
            throw new NotImplementedException();
        }

        public void GrantPrivileges(string name, string privilege, string table_name, string withGrantOption)
        {
            throw new NoPrivilegeException();
        }

        public void GrantPrivilegesOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns, string withGrantOption)
        {
            throw new NoPrivilegeException();
        }

        public void GrantSelectOnSpecificColumnsOfTableOrView(string name, string table_name, string columns, string withGrantOption)
        {
            throw new NoPrivilegeException();
        }

        public void GrantSystemPrivilegesToUser(string name, string privilege, string withAdminOption)
        {
            throw new NoPrivilegeException();
        }

        public void RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns)
        {
            throw new NoPrivilegeException();
        }

        public void RevokePrivilegesOfUserOnSpecificObjectType(string name, string privilege, string table_name)
        {
            throw new NoPrivilegeException();
        }

        public void RevokeSystemPrivilegesFromUser(string name, string privilege)
        {
            throw new NoPrivilegeException();
        }
    }
}
