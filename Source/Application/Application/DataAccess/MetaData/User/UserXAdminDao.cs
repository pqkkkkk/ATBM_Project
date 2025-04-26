using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.User
{
    class UserXAdminDao : IUserDao
    {
        public bool CheckExist(string type, string name)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public List<Model.User> LoadData()
        {
            throw new NotImplementedException();
        }

        public bool UpdatePassword(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
