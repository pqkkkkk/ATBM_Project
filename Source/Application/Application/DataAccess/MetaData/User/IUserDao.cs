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
        public void CreateUser(string username, string password);
        public void DeleteUser(string username);
        public void UpdatePassword(string username, string password);
    }
}
