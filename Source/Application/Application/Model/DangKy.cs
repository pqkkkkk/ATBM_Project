using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class DangKy : INotifyPropertyChanged
    {
        public string? maSV { get; set; }
        public string? maMM { get; set; }
        public int? diemTH { get; set; }
        public int? diemCT { get; set; }
        public int? diemCK { get; set; }
        public int? diemTK { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
