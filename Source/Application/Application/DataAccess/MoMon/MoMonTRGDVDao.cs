using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Application.DataAccess.MoMon
{
    class MoMonTRGDVDao : IMoMonDao
    {
        private OracleConnection sqlConnection;

        public MoMonTRGDVDao(OracleConnection sqlConnection)
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
            List<Model.MoMon> result = new List<Model.MoMon>();
            using (var cmd = new OracleCommand("X_ADMIN.X_ADMIN_Select_MOMON_Table_ForTRGDV", sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mm = new Model.MoMon
                        {
                            MAMM = reader["maMM"].ToString(),
                            MAHP = reader["maHP"].ToString(),
                            MAGV = reader["maGV"].ToString(),
                            HK = reader["hk"] != DBNull.Value ? Convert.ToInt32(reader["hk"]) : null,
                            NAM = reader["nam"] != DBNull.Value ? Convert.ToInt32(reader["nam"]) : null
                        };

                        result.Add(mm);
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
