using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PrivilegeViewModel : BaseViewModel, INotifyPropertyChanged
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

        public List<object> LoadData()
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            throw new NotImplementedException();
        }

        public bool CreateItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
