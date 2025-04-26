using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.MetaData.Role
{
    class RoleXAdminDao : IRoleDao
    {
        private OracleConnection sqlConnection;

        public RoleXAdminDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool CheckExist(string role)
        {
            bool check = false;
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_CheckRoleExist", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("roleName", OracleDbType.Varchar2).Value = role;
                    cmd.Parameters.Add("exist", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    check = Convert.ToBoolean(((OracleDecimal)cmd.Parameters["exist"].Value).ToInt32());
                    return check;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public bool CreateRole(string roleName)
         {

             if (CheckExist(roleName))
             {
                 return false;
             }
             try
             {
                 if (sqlConnection.State == ConnectionState.Closed)
                 {
                     sqlConnection.Open();
                 }
                 using (OracleCommand cmd = new OracleCommand("X_ADMIN_createRole", sqlConnection))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add("user_role", OracleDbType.Varchar2).Value = roleName;
                     cmd.ExecuteNonQuery();
                 }
                 return true;
             }
             catch (Exception e)
             {
                 throw new Exception(e.Message);
             }
             finally
             {
                 if (sqlConnection.State == ConnectionState.Open)
                 {
                     sqlConnection.Close();
                 }
             }
         }

        public bool DropRole(string roleName)
        {
            if (!CheckExist(roleName))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_dropRole", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_role", OracleDbType.Varchar2).Value = roleName;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DropRole] Lỗi khi xóa role '{roleName}': {e.Message}");
                return false;
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public List<Model.Role> GetAllRoles()
        {
            List<Model.Role> roles = new List<Model.Role>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_getAllRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_list", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Role role = new Model.Role()
                        {
                            name = reader["GRANTED_ROLE"].ToString(),
                            AdminOption = reader["ADMIN_OPTION"].ToString(),
                            defaultRole = reader["DEFAULT_ROLE"].ToString(),
                            osGranted = reader["OS_GRANTED"].ToString(),
                            common = reader["COMMON"].ToString(),
                            inherited = reader["INHERITED"].ToString()
                        };
                        if(role.name != null && role.name.StartsWith("XR_"))
                        {
                            role.name = role.name.Remove(0, 3);
                        }
                        roles.Add(role);
                    }
                    //reader.Close();
                    return roles;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public List<Model.Role> GetAllRolesWithRoleClass()
        {
            List<Model.Role> roles = new List<Model.Role>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("getAllRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_list", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Role role = new Model.Role()
                        {
                            name = reader.GetString(0),
                        };

                        roles.Add(role);
                    }
                    reader.Close();
                    return roles;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public List<string>? GetUserRoles(string username)
        {
            List<string> userRoles = new List<string>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_GetUserRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("role_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string role = reader.GetString(0);
                        userRoles.Add(role);
                    }
                    reader.Close();
                    return userRoles;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public void GrantRole(string username, string rolename, string withGrantOption)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_grantRole", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_name", OracleDbType.Varchar2).Value = rolename;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("withGrantOption", OracleDbType.Varchar2).Value = withGrantOption;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        public void RevokeRoleFromUser(string username, string rolename)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_revokeRoleFromUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_", OracleDbType.Varchar2).Value = rolename;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
