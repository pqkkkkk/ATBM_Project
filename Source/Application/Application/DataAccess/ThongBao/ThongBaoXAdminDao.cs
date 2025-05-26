using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ABI.System.Collections;
using Application.Model;
using Application.Views;
using Microsoft.UI.Xaml;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.ThongBao
{
    public class ThongBaoXAdminDao : IThongBaoDao
    {
        private OracleConnection sqlConnection;

        public ThongBaoXAdminDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool Add(object obj)
        {
            return true;
        }
        public bool SendNotification(string content, string label)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            try
            {
                using (OracleCommand cmd = new OracleCommand("SendNotification", sqlConnection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_noidung", OracleDbType.Varchar2).Value = content;
                    cmd.Parameters.Add("p_label", OracleDbType.Varchar2).Value = label;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
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
        public bool Delete(object obj)
        {
            throw new NotImplementedException();
        }

        public List<object> Load(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            List<Model.ThongBao> result = new List<Model.ThongBao>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_THONGBAO_Table", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tb = new Model.ThongBao
                        {
                            MATB = Convert.ToInt32(reader["MATB"]),
                            NOIDUNG = reader["NOIDUNG"].ToString(),
                            NGAYTB = DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYTB"])),
                        };

                        result.Add(tb);
                    }
                }
            }
            sqlConnection.Close();
            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            throw new NotImplementedException();
        }

        // Lấy các level
        public List<LabelComponent> GetAllLevels()
        {
            List<LabelComponent> levelList = new List<LabelComponent>();
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_GetLevels", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var label = new LabelComponent
                            {
                                LONG_NAME = reader["LONG_NAME"].ToString() ,
                                SHORT_NAME = reader["SHORT_NAME"].ToString(),
                                TYPE = "Level"
                            };
                            levelList.Add(label);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
            return levelList;
        }
        // Lấy các department
        public List<LabelComponent> GetAllDepartments()
        {
            List<LabelComponent> departmentList = new List<LabelComponent>();

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_GetDepartments", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var department = new LabelComponent
                            {
                                LONG_NAME = reader["LONG_NAME"].ToString(),
                                SHORT_NAME = reader["SHORT_NAME"].ToString(),
                                TYPE = "Department"
                            };
                            departmentList.Add(department);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
            return departmentList;
        }
        // lấy các group
        public List<LabelComponent> GetAllGroups()
        {
            List<LabelComponent> groupList = new List<LabelComponent>();    
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_GetGroups", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var group = new LabelComponent
                            {
                                LONG_NAME = reader["LONG_NAME"].ToString(),
                                SHORT_NAME = reader["SHORT_NAME"].ToString(),
                                TYPE = "Group"
                            };
                            groupList.Add(group);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
            return groupList;
        }
    }
}
