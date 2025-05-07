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
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
