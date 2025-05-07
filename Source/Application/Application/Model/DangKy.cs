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
        public string? maSV { get; set; }
        public string? maMM { get; set; }
        public int? diemTH { get; set; }
        public int? diemCT { get; set; }
        public int? diemCK { get; set; }
        public int? diemTK { get; set; }
        public bool? isInDB { get; set; }

        public DangKy()
        {
            isInDB = true;
            maMM = "Nhập mã mở môn";
            maSV = "Nhập mã sinh viên";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
