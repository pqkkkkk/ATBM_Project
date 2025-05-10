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
    class SinhVienGVDao : ISinhVienDao
    {
        private OracleConnection sqlConnection;
        public SinhVienGVDao(OracleConnection sqlConnection)
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
            using (var cmd = new OracleCommand("SELECT * FROM X_ADMIN.SINHVIEN", sqlConnection))
            {
                cmd.CommandType = CommandType.Text;
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
                            TINHTRANG = reader["tinhTrang"].ToString()
                        };

                        result.Add(sv);
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
