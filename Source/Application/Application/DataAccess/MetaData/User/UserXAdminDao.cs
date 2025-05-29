using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.User
{
    class UserXAdminDao : IUserDao
    {
        private OracleConnection sqlConnection;

        public UserXAdminDao(OracleConnection oracleConnection)
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
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_checkExistUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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

        public bool CreateUser(string username, string password, string role)
        {
            string actual_username = "X_" +  username.ToUpper();
            if (CheckExist("USER", actual_username))
            {
                return false;
            }
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_createUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("pwd", OracleDbType.Varchar2).Value = password;
                    cmd.Parameters.Add("p_role", OracleDbType.Varchar2).Value = role;
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    return true;
                }
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
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_deleteUser", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("user_name", OracleDbType.Varchar2).Value = username;
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    return true;
                }
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
            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_getAllUsers", sqlConnection))
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
                            //password = reader["password"].ToString(),
                            userId = Convert.ToString(reader["user_Id"]),
                            //accountStatus = reader["account_Status"].ToString(),
                            //defaultTablespace = reader["default_Tablespace"].ToString(),
                            created = DateOnly.FromDateTime(Convert.ToDateTime(reader["created"])),
                            //authenticationType = reader["authentication_Type"].ToString(),
                            common = reader["common"].ToString(),
                            //passwordChangeDate = reader["password_Change_Date"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["password_Change_Date"])) : null
                        };
                        if(user.username != null && user.username.StartsWith("X_"))
                        {
                            user.username = user.username.Remove(0,2);
                        }
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
                using (OracleCommand cmd = new OracleCommand("X_ADMIN_updatePassword", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("new_pwd", OracleDbType.Varchar2).Value = password;
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    return true;
                }
            }
            catch (System.Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }
    }
}
