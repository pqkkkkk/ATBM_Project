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
                            diemTH = Convert.ToInt16(reader["diemTH"]),
                            diemCT = Convert.ToInt16(reader["diemCT"]),
                            diemCK = Convert.ToInt16(reader["diemCK"]),
                            diemTK = Convert.ToInt16(reader["diemTK"])
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
