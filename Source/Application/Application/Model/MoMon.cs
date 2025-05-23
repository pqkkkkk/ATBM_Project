using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class MoMon : IPersistable, INotifyPropertyChanged
    {
        public string? MAMM {  get; set; }
        public string? MAHP { get; set; }
        public string? MAGV { get; set; }
        public int? HK { get; set; }
        public int? NAM { get; set; }
        public bool? isInDB { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public MoMon()
        {
            MAMM = "maMM";
            MAHP = "maHP";
            MAGV = "maGV";
            HK = 1;
            NAM = 2025;
        }
    }
}
