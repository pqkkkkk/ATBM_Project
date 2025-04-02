using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PrivilegeViewModel : INotifyPropertyChanged
    {
        public string selectObjectType { get; set; }
        public PrivilegeViewModel()
        {
            selectObjectType = "";
        }
        public void UpdateSelectedObjectType(string objectType)
        {
            selectObjectType = objectType;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
