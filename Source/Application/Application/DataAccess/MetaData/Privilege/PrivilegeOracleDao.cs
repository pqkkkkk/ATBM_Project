using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                sqlConnection.Open();
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
