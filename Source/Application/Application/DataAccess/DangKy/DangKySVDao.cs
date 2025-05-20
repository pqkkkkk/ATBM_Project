using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

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
                cmd.Parameters.Add("p_maSV", OracleDbType.Varchar2).Value = dk.maSV;
                cmd.Parameters.Add("p_maMM", OracleDbType.Varchar2).Value = dk.maMM;
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
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
                            maSV = reader["maSV"].ToString(),
                            maMM = reader["maMM"].ToString(),
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
            throw new NotImplementedException();
        }

    }
}
