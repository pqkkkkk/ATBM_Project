using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
<<<<<<< HEAD
    public class DangKy : IPersistable, INotifyPropertyChanged
=======

    public class DangKy : INotifyPropertyChanged, IPersistable
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
    {
        public string? maSV { get; set; }
        public string? maMM { get; set; }
        public double? diemTH { get; set; }
        public double? diemCT { get; set; }
        public double? diemCK { get; set; }
        public double? diemTK { get; set; }
        public bool? isInDB { get; set; }
<<<<<<< HEAD
=======

        public DangKy()
        {
            isInDB = true;
            maMM = "Nhập mã mở môn";
            maSV = "Nhập mã sinh viên";
        }
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
