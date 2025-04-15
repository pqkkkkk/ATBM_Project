using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class Privilege : INotifyPropertyChanged
    {
        public string? grantee { get; set; }
        public string? owner { get; set; }
        public string? tableName { get; set; }
        public string? columnName { get; set; }
        public string? grantor { get; set; }
        public string? privilege { get; set; }
        public string? type { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
