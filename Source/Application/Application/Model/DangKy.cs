using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class DangKy : IPersistable, INotifyPropertyChanged
    {
        public string? maSV { get; set; }
        public string? maMM { get; set; }
        public double? diemTH { get; set; }
        public double? diemCT { get; set; }
        public double? diemCK { get; set; }
        public double? diemTK { get; set; }
        public bool? isInDB { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
