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
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
