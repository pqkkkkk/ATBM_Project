using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class MoMon : INotifyPropertyChanged
    {
        public string? maMM {  get; set; }
        public string? maHP { get; set; }
        public string? maGV { get; set; }
        public int? hk { get; set; }
        public int? nam { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
