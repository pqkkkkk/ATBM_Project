using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

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
            if (CheckExist("USER", username))
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
    }
}
