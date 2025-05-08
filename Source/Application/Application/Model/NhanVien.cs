using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{

    public class NhanVien : INotifyPropertyChanged, IPersistable
    {
        public string? maNV { get; set; }
        public string? hoTen { get; set; }
        public string? phai { get; set; }
        public DateOnly? ngSinh { get; set; }
        public int? luong { get; set; }
        public int? phuCap { get; set; }
        public string? dt { get; set; }
        public string? vaiTro { get; set; }
        public string? maDV { get; set; }
        public bool? isInDB { get; set; }
        
        public NhanVien()
        {
            maNV = "";
            hoTen = "";
            phai = "";
            ngSinh = null;
            luong = null;
            phuCap = null;
            dt = "";
            vaiTro = "";
            maDV = "";
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
