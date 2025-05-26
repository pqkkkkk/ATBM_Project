using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class ThongBao : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int? MATB { get; set; }
        public DateOnly? NGAYTB { get; set; }
        public string? NOIDUNG { get; set; }
    }
}
