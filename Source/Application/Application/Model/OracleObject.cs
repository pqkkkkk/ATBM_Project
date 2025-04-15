using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class OracleObject : INotifyPropertyChanged
    {
        public string? owner { get; set; }
        public string? objectName { get; set; }
        public int? objectId { get; set; }
        public string? objectType { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
