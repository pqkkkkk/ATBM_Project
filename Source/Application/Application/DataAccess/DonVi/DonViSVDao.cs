using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;

namespace Application.DataAccess.DonVi
{
    class DonViSVDao : IDonViDao
    {
        public bool Add(object obj)
        {
            throw new NoPrivilegeException();
        }

        public bool Delete(object obj)
        {
            throw new NoPrivilegeException();
        }

        public List<object> Load(object obj)
        {
            throw new NoPrivilegeException();
        }

        public bool Update(object obj)
        {
            throw new NoPrivilegeException();
        }
    }
}
