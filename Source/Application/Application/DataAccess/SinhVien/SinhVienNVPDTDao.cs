using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.SinhVien
{
    class SinhVienNVPDTDao : ISinhVienDao
    {
        private OracleConnection sqlConnection;
        public SinhVienNVPDTDao(OracleConnection sqlConnection)
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
                            TINHTRANG = reader["tinhTrang"].ToString()
                        };
                        sv.isInDB = true;
                        result.Add(sv);
                    }
                }
            }

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                Model.SinhVien sinhVien = (Model.SinhVien)obj;
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_update_TT_SV", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("MaSV_", OracleDbType.Varchar2).Value = sinhVien.maSV;
                    cmd.Parameters.Add("TINHTRANG_", OracleDbType.Varchar2).Value = sinhVien.TINHTRANG;

                    var rowAffectedParam = new OracleParameter("ROW_AFFECTED", OracleDbType.Int32);
                    rowAffectedParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(rowAffectedParam);

                    cmd.ExecuteNonQuery();
                    int rowsAffected = ((OracleDecimal)rowAffectedParam.Value).ToInt32();
                    return rowsAffected > 0;
                }
            }
            catch (System.Exception ex)
            {
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
    }
}
