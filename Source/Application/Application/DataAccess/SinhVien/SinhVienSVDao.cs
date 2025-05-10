using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.SinhVien
{
    class SinhVienSVDao : ISinhVienDao
    {
        private OracleConnection sqlConnection;
        public SinhVienSVDao(OracleConnection sqlConnection)
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
            List<Model.SinhVien> result = new List<Model.SinhVien>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_SINHVIEN_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sv = new Model.SinhVien
                        {
                            maSV = reader["maSV"].ToString(),
                            hoTen = reader["hoTen"].ToString(),
                            phai = reader["phai"].ToString(),
                            ngSinh = DateOnly.FromDateTime(Convert.ToDateTime(reader["ngSinh"])),
                            dChi = reader["dChi"].ToString(),
                            dt = reader["dt"].ToString(),
                            khoa = reader["khoa"].ToString(),
                            TINHTRANG = reader["tinhTrang"].ToString(),
                            isInDB = true
                        };

                        result.Add(sv);
                    }
                }
            }

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            var sv = obj as Model.SinhVien;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Update_SINHVIEN_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_dChi", OracleDbType.Varchar2).Value = sv.dChi;
                cmd.Parameters.Add("p_dt", OracleDbType.Varchar2).Value = sv.dt;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
        }
    }
}
