using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public interface BaseViewModel
    {
        public List<object> LoadData();
        public void UpdateSelectedItem(object selectedItem);
        public bool CreateItem(object item);
        public bool UpdateItem(object item);
        public bool DeleteItem(object item);
    }
}
