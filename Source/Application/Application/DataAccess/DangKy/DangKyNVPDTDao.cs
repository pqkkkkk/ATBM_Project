using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.DangKy
{
    class DangKyNVPDTDao : IDangKyDao
    {
        private OracleConnection sqlConnection;
        public DangKyNVPDTDao(OracleConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public bool Add(object obj)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                Model.DangKy dk = (Model.DangKy)obj;
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_insert_DANGKY_NVPDT", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("MaSV_", dk.MASV));
                    cmd.Parameters.Add(new OracleParameter("MaMM_", dk.MAMM));
                    cmd.ExecuteNonQuery();
                }
                sqlConnection.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool Delete(object obj)
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                Model.DangKy dk = (Model.DangKy)obj;
                using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_delete_DANGKY_NVPDT", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("MaSV_", dk.MASV));
                    cmd.Parameters.Add(new OracleParameter("MaMM_", dk.MAMM));

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

        public List<object> Load(object obj)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            List<Model.DangKy> result = new List<Model.DangKy>();
            using (var cmd = new OracleCommand("SELECT * FROM X_ADMIN.DANGKY", sqlConnection))
            {
                cmd.CommandType = CommandType.Text;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dk = new Model.DangKy
                        {
                            MASV = reader["maSV"].ToString(),
                            MAMM = reader["maMM"].ToString(),
                            diemTH = reader["diemTH"] == DBNull.Value ? (double?)null : Convert.ToDouble(reader["diemTH"]),
                            diemCT = reader["diemCT"] == DBNull.Value ? (double?)null : Convert.ToDouble(reader["diemCT"]),
                            diemCK = reader["diemCK"] == DBNull.Value ? (double?)null : Convert.ToDouble(reader["diemCK"]),
                            diemTK = reader["diemTK"] == DBNull.Value ? (double?)null : Convert.ToDouble(reader["diemTK"]),
                        }
                        ;
                        dk.isInDB = true;
                        result.Add(dk);
                    }
                }
            }
            sqlConnection.Close();
            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            return true;
        }
    }
}
