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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                sqlConnection.Close();
                return false;
            }
        }

        public List<object> LoadData()
        {
            List<object> userList = new List<object>();
            try
            {
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
                            string username = reader.GetString(0);
                            string password = "null";
                            userList.Add(new { Username = username, Password = password });
                        }
                        reader.Close();
                    }
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                return userList;
            }
            return userList;
        }
    }
}
