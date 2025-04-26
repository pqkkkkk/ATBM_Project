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
        public string? AdminOption { get; set; }
        public string? defaultRole { get; set; }
        public string? osGranted { get; set; }
        public string? common { get; set; }
        public string? inherited { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
