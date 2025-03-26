using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class UserPrivilege : INotifyPropertyChanged
    {
        public int? objectName { get; set; }
        public string? objectType { get; set; }
        public string? privilege { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
