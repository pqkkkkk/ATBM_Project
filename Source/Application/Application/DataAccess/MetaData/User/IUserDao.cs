using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess.MetaData.User
{
    public interface IUserDao
    {
        public bool CheckExist(string type, string name);
        public bool CreateUser(string username, string password, string role);
        public bool DeleteUser(string username);
        public bool UpdatePassword(string username, string password);
        public List<Model.User> LoadData();
    }
}
