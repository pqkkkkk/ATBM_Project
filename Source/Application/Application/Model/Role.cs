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
        public string? roleId { get; set; }
        public string? passwordRequired { get; set; }
        public string? authenticationType { get; set; }
        public string? common { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
