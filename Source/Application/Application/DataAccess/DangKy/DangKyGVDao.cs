using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.DangKy
{
    class DangKyGVDao : IDangKyDao
    {
        private OracleConnection sqlConnection;
        public DangKyGVDao(OracleConnection sqlConnection)
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

            return result.Cast<object>().ToList();
        }

        public bool Update(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
