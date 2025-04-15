using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.MetaData.Privilege
{
    public class PrivilegeOracleDao : IPrivilegeDao
    {
        private OracleConnection sqlConnection;

        public PrivilegeOracleDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public Dictionary<string, List<string>> GetUserPrivileges(string name)
        {
            Dictionary<string, List<string>> userPrivileges = new Dictionary<string, List<string>>();
            try
            {
                if(sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("GetUserPrivileges", sqlConnection))
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
                if (sqlConnection.State == ConnectionState.Closed)
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
        public void RevokePrivilegesOfUserOnSpecificObjectType(string name, string privilege, string table_name)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
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
                if (sqlConnection.State == ConnectionState.Closed)
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
        public List<Model.Privilege> GetPrivilegesOfUserOnSpecificObjectType(string name, string objectType)
        {
            List<Model.Privilege> privileges = new List<Model.Privilege>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("getPrivilegesOnObjectType", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("object_type", OracleDbType.Varchar2).Value = objectType;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Privilege privilege = new Model.Privilege()
                        {
                            grantee = reader["GRANTEE"].ToString(),
                            owner = reader["OWNER"].ToString(),
                            tableName = reader["TABLE_NAME"].ToString(),
                            grantor = reader["GRANTOR"].ToString(),
                            privilege = reader["PRIVILEGE"].ToString(),
                            type = reader["TYPE"].ToString(),
                        };
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
        public List<Model.ColumnOfObject> GetColumns(string objectName)
        {
            List<Model.ColumnOfObject> colums = new List<Model.ColumnOfObject>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("getColumns", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("object_name", OracleDbType.Varchar2).Value = objectName;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.ColumnOfObject column = new ColumnOfObject()
                        {
                            columnName = reader["COLUMN_NAME"].ToString(),
                        };

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

        public List<Model.Role> GetAllRolesOfUser(string username)
        {
            List<Application.Model.Role> userRoles = new List<Model.Role>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("GetUserRoles", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("role_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string role = reader["GRANTED_ROLE"].ToString();

                        userRoles.Add(new Model.Role()
                        {
                            name = role,
                        });
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

        public List<Model.Privilege> GetSystemPrivilegesOfUser(string name)
        {
            List<Model.Privilege> privileges = new List<Model.Privilege>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("GetSystemPrivilegesOfUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Privilege privilege = new Model.Privilege()
                        {
                            grantee = reader["GRANTEE"].ToString(),
                            privilege = reader["PRIVILEGE"].ToString(),
                            type = "systemprivilege",
                        };
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

        public List<OracleObject> GetAllInstanceOfSpecificObject(string objectType)
        {
            List<Model.OracleObject> result = new List<Model.OracleObject>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("GetAllInstanceOfSpecificObject", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("type", OracleDbType.Varchar2).Value = objectType;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.OracleObject oracleObject = new Model.OracleObject()
                        {
                            owner = reader["OWNER"].ToString(),
                            objectName = reader["OBJECT_NAME"].ToString(),
                            objectId = reader["OBJECT_ID"] != DBNull.Value ? Convert.ToInt32(reader["OBJECT_ID"]) : (int?)null,
                            objectType = reader["OBJECT_TYPE"].ToString(),
                        };
                        result.Add(oracleObject);
                    }
                    reader.Close();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void GrantPrivilegesOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns, string withGrantOption)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("GrantPrivilegesOnSpecificColumnsOfTableOrView", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramWithGrantOption = new OracleParameter("p_withGrantOption", OracleDbType.Varchar2);
                    paramWithGrantOption.Value = withGrantOption;
                    cmd.Parameters.Add(paramWithGrantOption);

                    OracleParameter paramPrivilege = new OracleParameter("p_privilege", OracleDbType.Varchar2);
                    paramPrivilege.Value = privilege;
                    cmd.Parameters.Add(paramPrivilege);

                    OracleParameter paramUser = new OracleParameter("p_user", OracleDbType.Varchar2);
                    paramUser.Value = name;
                    cmd.Parameters.Add(paramUser);

                    OracleParameter paramColumns = new OracleParameter("p_columns", OracleDbType.Varchar2);
                    paramColumns.Value = columns;
                    cmd.Parameters.Add(paramColumns);

                    OracleParameter paramObject = new OracleParameter("p_object", OracleDbType.Varchar2);
                    paramObject.Value = table_name;
                    cmd.Parameters.Add(paramObject);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public List<Model.Privilege> GetColumnPrivilegesOfUser(string name)
        {
            List<Model.Privilege> privileges = new List<Model.Privilege>();
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("GetColumnPrivilegesOfUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("result_", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Model.Privilege privilege = new Model.Privilege()
                        {
                            grantee = reader["GRANTEE"].ToString(),
                            privilege = reader["PRIVILEGE"].ToString(),
                            owner = reader["OWNER"].ToString(),
                            tableName = reader["TABLE_NAME"].ToString(),
                            columnName = reader["COLUMN_NAME"].ToString(),
                            grantor = reader["GRANTOR"].ToString(),
                            type = "columnprivilege",
                        };
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

        public void RevokeSystemPrivilegesFromUser(string name, string privilege)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                using (OracleCommand cmd = new OracleCommand("RevokeSystemPrivilegesFromUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("privilege_", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("name_", OracleDbType.Varchar2).Value = name;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(string name, string privilege, string table_name, string columns)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();

                using (OracleCommand cmd = new OracleCommand("RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_privilege", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("p_user", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("p_columns", OracleDbType.Varchar2).Value = columns;
                    cmd.Parameters.Add("p_object", OracleDbType.Varchar2).Value = table_name;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
