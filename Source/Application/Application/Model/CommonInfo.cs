using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class CommonInfo : INotifyPropertyChanged
    {
        public string? name { get; set; }
        public string? objectType { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
