using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace Application.Model
{
    public class TableAction : INotifyPropertyChanged
    {
        public string? actionName { get; set; }
        public bool isSelected { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
