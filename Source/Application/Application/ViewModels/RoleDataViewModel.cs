using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.ViewModels
{
    public class RoleDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<Role> itemList { get; set; }
        public Role? selectedRole { get; set; }

        public RoleDataViewModel()
        {
            itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
            selectedRole = null;
        }
        public bool CreateItem(object item)
        {
            throw new NotImplementedException();
        }

        public int DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedItem = (Role)selectedItem;
        }

        public List<object> LoadData()
        {
            List<Role> userList = new List<Role>();

            userList.Add(new Role()
            {
                name = "NVCB"
            });
            userList.Add(new Role()
            {
                name = "GV"
            });
            userList.Add(new Role()
            {
                name = "SV"
            });

            return userList.Cast<object>().ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
