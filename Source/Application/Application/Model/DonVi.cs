using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class DonVi : INotifyPropertyChanged
    {
        public string? maDV { get; set; }
        public string? tenDV { get; set; }
        public string? loaiDV { get; set; }
        public string? trgDV { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
