using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.System;

namespace Application.Exception
{
    class NoPrivilegeException : System.Exception
    {
        public NoPrivilegeException(string message) : base(message)
        {
        }
        public NoPrivilegeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
        public NoPrivilegeException()
        {
        }
    }
}
