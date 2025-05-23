using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{

    public class DangKy : INotifyPropertyChanged, IPersistable
    {
        public string? MASV { get; set; }
        public string? MAMM { get; set; }
        public double? diemTH { get; set; }
        public double? diemCT { get; set; }
        public double? diemCK { get; set; }
        public double? diemTK { get; set; }
        public bool? isInDB { get; set; }

        public DangKy()
        {
            isInDB = true;
            MAMM = "Nhập mã mở môn";
            MASV = "Nhập mã sinh viên";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
