using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MoMon
{
    class MoMonGVDao : IMoMonDao
    {
        private OracleConnection sqlConnection;
        public MoMonGVDao(OracleConnection sqlConnection)
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
            using (var cmd = new OracleCommand("SELECT * FROM X_ADMIN.view_GV_MM", sqlConnection))
            {
                cmd.CommandType = CommandType.Text;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mm = new Model.MoMon
                        {
                            maMM = reader["maMM"].ToString(),
                            maHP = reader["maHP"].ToString(),
                            maGV = reader["maGV"].ToString(),
                            hk = reader["hk"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["hk"]),
                            nam = reader["nam"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["nam"]),
                        }
                        ;
                        mm.isInDB = true;
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
