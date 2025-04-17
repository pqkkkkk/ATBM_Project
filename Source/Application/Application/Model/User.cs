using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class User : INotifyPropertyChanged
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? userId { get; set; }
        public string? accountStatus { get; set; }
        public string? defaultTablespace { get; set; }
        public DateOnly? created { get; set; }
        public string? authenticationType { get; set; }
        public string? common { get; set; }
        public DateOnly? passwordChangeDate { get; set; }

        public User()
        {
            username = "";
            password = "";
        }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
