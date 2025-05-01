using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess
{
    public interface IBaseDao
    {
        bool Add(object obj);
        bool Update(object obj);
        bool Delete(object obj);
        List<object> Load(object obj);
    }
}
