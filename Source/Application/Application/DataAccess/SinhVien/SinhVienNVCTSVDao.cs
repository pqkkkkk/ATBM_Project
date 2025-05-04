using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.SinhVien
{
    class SinhVienNVCTSVDao : ISinhVienDao
    {
        private OracleConnection sqlConnection;

        public SinhVienNVCTSVDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool Add(object obj)
        {
            var sv = obj as Model.SinhVien;

            if(sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Insert_SINHVIEN_Table_ForNVCTSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = sv.maSV;
                cmd.Parameters.Add("p_hoTen", OracleDbType.Varchar2).Value = sv.hoTen;
                cmd.Parameters.Add("p_phai", OracleDbType.Varchar2).Value = sv.phai;
                cmd.Parameters.Add("p_ngSinh", OracleDbType.Date).Value = sv.ngSinh?.ToDateTime(new TimeOnly(0, 0));
                cmd.Parameters.Add("p_dChi", OracleDbType.Varchar2).Value = sv.dChi;
                cmd.Parameters.Add("p_dt", OracleDbType.Varchar2).Value = sv.dt;
                cmd.Parameters.Add("p_khoa", OracleDbType.Varchar2).Value = sv.khoa;
                cmd.ExecuteNonQuery();
                return true;
            }
            
        }

        public bool Delete(object obj)
        {
            var sv = obj as Model.SinhVien;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Delete_SINHVIEN_Table_ForNVCTSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("rowAffected", OracleDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = sv.maSV;
                cmd.ExecuteNonQuery();

                var outputParam = cmd.Parameters["rowAffected"].Value;
                int rowAffected = outputParam != null && outputParam != DBNull.Value
                    ? ((OracleDecimal)outputParam).ToInt32() : 0;

                sqlConnection.Close();
                return true;
            }
        }

        public List<object> Load(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            List<Model.SinhVien> result = new List<Model.SinhVien>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_SINHVIEN_Table_ForNVCTSV", sqlConnection))
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
                            tinhTrang = reader["tinhTrang"].ToString(),
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

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Update_SINHVIEN_Table_ForNVCTSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = sv.maSV;
                cmd.Parameters.Add("p_hoTen", OracleDbType.Varchar2).Value = sv.hoTen;
                cmd.Parameters.Add("p_phai", OracleDbType.Varchar2).Value = sv.phai;
                cmd.Parameters.Add("p_ngSinh", OracleDbType.Date).Value = sv.ngSinh?.ToDateTime(new TimeOnly(0, 0));
                cmd.Parameters.Add("p_dChi", OracleDbType.Varchar2).Value = sv.dChi;
                cmd.Parameters.Add("p_dt", OracleDbType.Varchar2).Value = sv.dt;
                cmd.Parameters.Add("p_khoa", OracleDbType.Varchar2).Value = sv.khoa;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
        }
    }
}
