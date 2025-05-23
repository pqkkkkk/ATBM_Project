using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Application.DataAccess.DangKy
{
    class DangKySVDao : IDangKyDao
    {
        private OracleConnection sqlConnection;
        public DangKySVDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool Add(object obj)
        {
            var dk = obj as Model.DangKy;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Insert_DANGKY_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = dk.MASV;
                cmd.Parameters.Add("p_maMM", OracleDbType.Varchar2).Value = dk.MAMM;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
        }

        public bool Delete(object obj)
        {
            var dk = obj as Model.DangKy;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Delete_DANGKY_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = dk.maSV;
                cmd.Parameters.Add("p_maMM", OracleDbType.Varchar2).Value = dk.maMM;
                cmd.Parameters.Add("p_row_affected", OracleDbType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int rowAffected = ((OracleDecimal)cmd.Parameters["p_row_affected"].Value).ToInt32();
                sqlConnection.Close();

                return rowAffected > 0;

            }
        }

        public List<object> Load(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            List<Model.DangKy> result = new List<Model.DangKy>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_DANGKY_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dk = new Model.DangKy
                        {
                            MASV = reader["maSV"].ToString(),
                            MAMM = reader["maMM"].ToString(),
                            diemTH = reader["diemTH"] != DBNull.Value ? Convert.ToInt16(reader["diemTH"]) : null,
                            diemCT = reader["diemCT"] != DBNull.Value ?  Convert.ToInt16(reader["diemCT"]) : null,
                            diemCK = reader["diemCK"] != DBNull.Value ? Convert.ToInt16(reader["diemCK"]) : null,
                            diemTK = reader["diemTK"] != DBNull.Value ? Convert.ToInt16(reader["diemTK"]) : null
                        };

                        result.Add(dk);
                    }
                }
            }

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            var dk = obj as Model.DangKy;

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            using (OracleCommand cmd = new OracleCommand("X_ADMIN.X_ADMIN_Update_DANGKY_Table_ForSV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = dk.maSV;
                cmd.Parameters.Add("p_maMM", OracleDbType.Varchar2).Value = dk.maMM;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
        }

    }
}
