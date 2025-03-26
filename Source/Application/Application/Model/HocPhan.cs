using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class HocPhan : INotifyPropertyChanged
    {
        public string? maHP { get; set; }
        public string? tenHP { get; set; }
        public int? soTC { get; set; }
        public int? stlt { get; set; }
        public int? stth { get; set; }
        public string? maDV { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
