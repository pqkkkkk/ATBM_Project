using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Role : INotifyPropertyChanged
    {
        public string? name { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
