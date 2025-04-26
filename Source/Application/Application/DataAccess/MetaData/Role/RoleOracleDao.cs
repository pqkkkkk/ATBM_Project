using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.MetaData.Role
{
    public class RoleOracleDao : IRoleDao
    {
        private OracleConnection sqlConnection;

        public RoleOracleDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool CheckExist(string type, string name)
        {
            bool check = false;
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("CheckExist", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("type_", OracleDbType.Varchar2).Value = type;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
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

        public bool CheckExist(string role)
        {
            throw new NotImplementedException();
        }

        public bool CreateRole(string roleName)
        {
            if (CheckExist("ROLE", roleName))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("createRole", sqlConnection))
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
            if (!CheckExist("ROLE", roleName))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("dropRole", sqlConnection))
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
                using (OracleCommand cmd = new OracleCommand("getAllRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_list", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Role role = new Model.Role()
                        {
                            name = reader["role"].ToString(),
                            //roleId = reader["role_id"].ToString(),
                            //passwordRequired = reader["password_required"].ToString(),
                            //authenticationType = reader["authentication_type"].ToString(),
                            common = reader["common"].ToString()
                        };
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
                if(sqlConnection.State == ConnectionState.Closed)
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

        public List<string> GetUserRoles(string username)
        {
            List<string> userRoles = new List<string>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("GetUserRoles", sqlConnection))
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
                using (OracleCommand cmd = new OracleCommand("grantRole", sqlConnection))
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
                using (OracleCommand cmd = new OracleCommand("revokeRoleFromUser", sqlConnection))
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
