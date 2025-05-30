using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.DangKy
{
    class DangKyNVPKTDao : IDangKyDao
    {
        private OracleConnection sqlConnection;
        public DangKyNVPKTDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool Add(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object obj)
        {
            throw new NotImplementedException();
        }

        public List<object> Load(object obj)
        {
            if(sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            List<Model.DangKy> result = new List<Model.DangKy>();
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_SELECT_DANGKY_TABLE_FOR_NVPKT", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dk = new Model.DangKy
                            {
                                maMM = reader["MAMM"].ToString(),
                                maSV = reader["MASV"].ToString(),
                                diemTH = reader["DIEMTH"] == DBNull.Value ? null : Convert.ToDouble(reader["DIEMTH"]),
                                diemCT = reader["DIEMTH"] == DBNull.Value ? null : Convert.ToDouble(reader["DIEMCT"]),
                                diemCK = reader["DIEMTH"] == DBNull.Value ? null : Convert.ToDouble(reader["DIEMCK"]),
                                diemTK = reader["DIEMTH"] == DBNull.Value ? null : Convert.ToDouble(reader["DIEMTK"]),
                                isInDB = true
                            };

                            result.Add(dk);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_UPDATE_DANGKY_TABLE_FOR_NVPKT", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dangKy = (Model.DangKy)obj;
                    cmd.Parameters.Add("p_MAMM", OracleDbType.Varchar2).Value = dangKy.maMM;
                    cmd.Parameters.Add("p_MASV", OracleDbType.Varchar2).Value = dangKy.maSV;
                    cmd.Parameters.Add("p_DIEMTH", OracleDbType.Double).Value = dangKy.diemTH;
                    cmd.Parameters.Add("p_DIEMCT", OracleDbType.Double).Value = dangKy.diemCT;
                    cmd.Parameters.Add("p_DIEMCK", OracleDbType.Double).Value = dangKy.diemCK;
                    cmd.Parameters.Add("p_DIEMTK", OracleDbType.Double).Value = dangKy.diemTK;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }
    }
}
