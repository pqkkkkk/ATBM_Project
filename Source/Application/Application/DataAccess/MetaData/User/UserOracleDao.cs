using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Application.Model;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.MetaData.User
{
    class UserOracleDao : IUserDao
    {
        private OracleConnection sqlConnection;

        public UserOracleDao(OracleConnection oracleConnection)
        {
            sqlConnection = oracleConnection;
        }
        public bool CheckExist(string type, string name)
        {
            bool check = false;
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
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
                    var oracleValue = (OracleDecimal)cmd.Parameters["exist"].Value;
                    check = oracleValue.ToInt32() == 1;
                    sqlConnection.Close();
                    return check;
                }
            }
            catch (System.Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }
        public bool CreateUser(string username, string password)
        {
            if (CheckExist("USER", username))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("createUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("pwd", OracleDbType.Varchar2).Value = password;
                    cmd.ExecuteNonQuery();
                }
                sqlConnection.Close();
                return true;
            }
            catch (System.Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }
        public bool DeleteUser(string username)
        {
            if (!CheckExist("USER", username))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("deleteUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.ExecuteNonQuery();
                }
                sqlConnection.Close();
                return true;
            }
            catch (System.Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }
        public bool UpdatePassword(string username, string password)
        {
            if (!CheckExist("USER", username))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("updatePassword", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("new_pwd", OracleDbType.Varchar2).Value = password;
                    cmd.ExecuteNonQuery();
                }
                sqlConnection.Close();
                return true;
            }
            catch (System.Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }

        public List<Model.User> LoadData()
        {
            List<Model.User> userList = new List<Model.User>();

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            using (OracleCommand cmd = new OracleCommand("getAllUsers", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("user_list", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Model.User user = new Model.User()
                        {
                            username = reader["username"].ToString(),
                            password = reader["password"].ToString(),
                            userId = Convert.ToString(reader["user_Id"]),
                            accountStatus = reader["account_Status"].ToString(),
                            defaultTablespace = reader["default_Tablespace"].ToString(),
                            created = DateOnly.FromDateTime(Convert.ToDateTime(reader["created"])),
                            authenticationType = reader["authentication_Type"].ToString(),
                            common = reader["common"].ToString(),
                            passwordChangeDate = reader["password_Change_Date"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["password_Change_Date"])) : null
                        };
                        userList.Add(user);
                    }
                    reader.Close();
                }
            }
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
          
            return userList;
        }
    }
}
