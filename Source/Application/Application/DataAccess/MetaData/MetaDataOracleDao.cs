using System;
using System.Collections.Generic;
using System.Data;
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
        public bool CheckExist(string type, string name)
        {
            bool check = false;
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("CheckExist", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("type_", OracleDbType.Varchar2).Value = type;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("exist", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    check = Convert.ToBoolean(cmd.Parameters["exist"].Value);
                    return check;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void CreateUser(string username, string password)
        {
            if(CheckExist("USER", username))
            {
                throw new Exception("User already exists");
            }
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("createUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("pwd", OracleDbType.Varchar2).Value = password;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void DeleteUser(string username)
        {
            if (!CheckExist("USER", username))
            {
                throw new Exception("User does not exist");
            }
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("deleteUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdatePassword(string username, string password)
        {
            if (!CheckExist("USER", username))
            {
                throw new Exception("User does not exist");
            }
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("updatePassword", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("new_pwd", OracleDbType.Varchar2).Value = password;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void CreateRole(string roleName)
        {
            if (CheckExist("ROLE", roleName))
            {
                throw new Exception("Role already exists");
            }
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("createRole", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_role", OracleDbType.Varchar2).Value = roleName;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void DropRole(string roleName)
        {
            if (!CheckExist("ROLE", roleName))
            {
                throw new Exception("Role does not exist");
            }
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("dropRole", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_role", OracleDbType.Varchar2).Value = roleName;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("getAllRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("role_list", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string role = reader.GetString(0);
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
        }
        public List<string> GetUserRoles(string username)
        {
            List<string> userRoles = new List<string>();
            try
            {
                sqlConnection.Open();
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
        }
        public void GrantRole(string username, string rolename, string withGrantOption)
        {
            try
            {
                sqlConnection.Open();
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
        }
        public void RevokeRoleFromUser(string username, string rolename)
        {
            try
            {
                sqlConnection.Open();
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
        }
        public Dictionary<string, List<string>> GetUserPrivileges(string name)
        {
            Dictionary<string, List<string>> userPrivileges = new Dictionary<string, List<string>>();
            try
            {
                sqlConnection.Open();
                using(OracleCommand cmd = new OracleCommand("GetUserPrivileges", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(2);
                        string privilege = reader.GetString(4);
                        if (userPrivileges.ContainsKey(tableName))
                        {
                            userPrivileges[tableName].Add(privilege);
                        }
                        else
                        {
                            userPrivileges[tableName] = new List<string> { privilege };
                        }
                    }
                    reader.Close();
                    return userPrivileges;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void GrantPrivileges(string name, string privilege, 
            string table_name, string withGrantOption)
        {
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("grantPrivileges", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("privilege_", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("object_", OracleDbType.Varchar2).Value = table_name;
                    cmd.Parameters.Add("withGrantOption", OracleDbType.Varchar2).Value = withGrantOption;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void RevokePrivilegesFromUser(string name, string privilege, string table_name)
        {
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("revokePrivilegesFromUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("privilege_", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("object_", OracleDbType.Varchar2).Value = table_name;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void CreateView(string viewName, string columns, string tableName, string condition)
        {
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("createView", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("view_name", OracleDbType.Varchar2).Value = viewName;
                    cmd.Parameters.Add("select_columns", OracleDbType.Varchar2).Value = columns;
                    cmd.Parameters.Add("table_name", OracleDbType.Varchar2).Value = tableName;
                    cmd.Parameters.Add("condition_", OracleDbType.Varchar2).Value = condition;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<string> GetPrivilegesObjectType(string name, string objectType)
        {
            List<string> privileges = new List<string>();
            try
            {
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("getPrivilegesObjectType", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("object_type", OracleDbType.Varchar2).Value = objectType;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string privilege = reader.GetString(4);
                        privileges.Add(privilege);
                    }
                    reader.Close();
                    return privileges;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<string> GetColumns(string objectName)
        {
            List<string> colums = new List<string>();
            try 
            { 
                sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("getColums", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("object_name", OracleDbType.Varchar2).Value = objectName;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string column = reader.GetString(0);
                        colums.Add(column);
                    }
                    reader.Close();
                    return colums;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
