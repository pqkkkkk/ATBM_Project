using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class SinhVien : IPersistable, INotifyPropertyChanged
    {
        public string? maSV { get; set; }
        public string? hoTen { get; set; }
        public string? phai { get; set; }
        public DateOnly? ngSinh { get; set; }
        public string? dChi { get; set; }
        public string? dt { get; set; }
        public string? khoa { get; set; }
        public string? tinhTrang { get; set; }
        public bool? isInDB { get; set; }
<<<<<<< HEAD
=======

        public SinhVien()
        {
            maSV = "Mã SV";
            hoTen = "Họ tên";
            phai = "Phái";
            ngSinh = DateOnly.FromDateTime(DateTime.Now);
            dChi = "Địa chỉ";
            dt = "Điện thoại";
            khoa = "Khoa";
            tinhTrang = "Tình trạng";
        }
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
