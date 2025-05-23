using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.NhanVien
{
    internal class NhanVienNVCBDao : INhanVienDao
    {
        private OracleConnection sqlConnection;
        public NhanVienNVCBDao(OracleConnection sqlConnection)
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
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            List<Model.NhanVien> result = new List<Model.NhanVien>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_NHANVIEN_ForNVCB", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var nv = new Model.NhanVien
                        {
                            maNV = reader["MANV"].ToString(),
                            hoTen = reader["hoTen"].ToString(),
                            phai = reader["phai"].ToString(),
                            ngSinh = Convert.ToDateTime(reader["ngSinh"]),
                            luong = reader["luong"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["luong"]),
                            phuCap = reader["phuCap"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["phuCap"]),
                            dt = reader["dt"].ToString(),
                            vaiTro = reader["vaiTro"].ToString(),
                            maDV = reader["maDV"].ToString()
                        }
                        ;
                        nv.isInDB = true;
                        result.Add(nv);
                    }
                }
            }

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            try
            {
                Model.NhanVien nv = (Model.NhanVien)obj;
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Update_NHANVIEN_ForNVCB", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("newDt", OracleDbType.Varchar2).Value = nv.dt;

                    var rowParam = cmd.Parameters.Add("ROW_AFFECTED", OracleDbType.Int32);
                    rowParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    int rowsAffected = ((OracleDecimal)rowParam.Value).ToInt32();

                    sqlConnection.Close();
                    return rowsAffected > 0;
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
