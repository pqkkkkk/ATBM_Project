using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.NhanVien
{
    class NhanVienNVTCHCDao : INhanVienDao
    {
        private OracleConnection sqlConnection;
        public NhanVienNVTCHCDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool Add(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_INSERT_NHANVIEN_TABLE_FOR_NVTCHC", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var nv = (Model.NhanVien)obj;
                    cmd.Parameters.Add("MaNLD", OracleDbType.Varchar2).Value = nv.maNV;
                    cmd.Parameters.Add("HoTen", OracleDbType.Varchar2).Value = nv.hoTen;
                    cmd.Parameters.Add("PHAI", OracleDbType.Varchar2).Value = nv.phai;
                    cmd.Parameters.Add("NgaySinh", OracleDbType.Date).Value = nv.ngSinh;
                    cmd.Parameters.Add("Luong", OracleDbType.Int32).Value = nv.luong;
                    cmd.Parameters.Add("PhuCap", OracleDbType.Int32).Value = nv.phuCap;
                    cmd.Parameters.Add("SDT", OracleDbType.Varchar2).Value = nv.dt;
                    cmd.Parameters.Add("VaiTro", OracleDbType.Varchar2).Value = nv.vaiTro;
                    cmd.Parameters.Add("MaDV", OracleDbType.Varchar2).Value = nv.maDV;

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

        public bool Delete(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_DELETE_NHANVIEN_TABLE_FOR_NVTCHC", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var nv = (Model.NhanVien)obj;
                    cmd.Parameters.Add("MaNLD", OracleDbType.Varchar2).Value = nv.maNV;

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

        public List<object> Load(object obj)
        {
            if(sqlConnection.State != ConnectionState.Open )
            {
                sqlConnection.Open();
            }
            List<Model.NhanVien> result = new List<Model.NhanVien>();
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_SELECT_NHANVIEN_TABLE_FOR_NVTCHC", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nv = new Model.NhanVien
                            {
                                maNV = reader["maNV"].ToString(),
                                hoTen = reader["hoTen"].ToString(),
                                phai = reader["phai"].ToString(),
                                ngSinh = Convert.ToDateTime(reader["ngSinh"]),
                                luong = reader["luong"] != DBNull.Value ? Convert.ToInt32(reader["luong"]) : 0,
                                phuCap = reader["phuCap"] != DBNull.Value ? Convert.ToInt32(reader["phuCap"]) : 0,
                                dt = reader["dt"].ToString(),
                                vaiTro = reader["vaiTro"].ToString(),
                                maDV = reader["maDV"].ToString(),
                                isInDB = true
                            };
                            result.Add(nv);
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
            if(sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            try
            {
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_UPDATE_NHANVIEN_TABLE_FOR_NVTCHC", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var nv = (Model.NhanVien)obj;
                    cmd.Parameters.Add("MaNLD", OracleDbType.Varchar2).Value = nv.maNV;
                    cmd.Parameters.Add("p_hoten", OracleDbType.Varchar2).Value = nv.hoTen;
                    cmd.Parameters.Add("p_PHAI", OracleDbType.Varchar2).Value = nv.phai;
                    cmd.Parameters.Add("NgaySinh", OracleDbType.Date).Value = nv.ngSinh;
                    cmd.Parameters.Add("p_luong", OracleDbType.Int32).Value = nv.luong;
                    cmd.Parameters.Add("p_phuCap", OracleDbType.Int32).Value = nv.phuCap;
                    cmd.Parameters.Add("SDT", OracleDbType.Varchar2).Value = nv.dt;
                    cmd.Parameters.Add("p_VaiTro", OracleDbType.Varchar2).Value = nv.vaiTro;
                    cmd.Parameters.Add("p_MaDV", OracleDbType.Varchar2).Value = nv.maDV;

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
