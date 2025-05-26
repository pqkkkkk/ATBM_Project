using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class LabelComponent:INotifyPropertyChanged
    {
        public string? SHORT_NAME { get; set; }
        public string? LONG_NAME { get; set; }
        public string? TYPE { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
