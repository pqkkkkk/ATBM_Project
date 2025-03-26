using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class UserMetaData : INotifyPropertyChanged
    {
        public List<string>? roles { get; set; }
        public string? username { get; set; }
        public List<UserPrivilege>? privileges { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
