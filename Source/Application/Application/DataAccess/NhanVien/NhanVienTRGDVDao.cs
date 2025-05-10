using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.NhanVien
{
    class NhanVienTRGDVDao : INhanVienDao
    {
        private OracleConnection sqlConnection;

        public NhanVienTRGDVDao(OracleConnection sqlConnection)
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
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_NHANVIEN_Table_ForTRGDV", sqlConnection))
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
                            ngSinh = reader["ngSinh"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["ngSinh"])) : null,
                            DT = reader["dt"].ToString(),
                            vaiTro = reader["vaiTro"].ToString(),
                            maDV = reader["maDV"].ToString()
                        };

                        result.Add(nv);
                    }
                }
            }

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
