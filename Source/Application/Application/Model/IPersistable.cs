using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public interface IPersistable
    {
        bool? isInDB { get; set; }
    }
}
