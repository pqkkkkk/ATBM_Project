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
    public class UserDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<User> itemList { get; set; }
        public User? selectedUser { get; set; }

        public UserDataViewModel()
        {
            itemList = new ObservableCollection<User>(LoadData().Cast<User>());
            selectedUser = null;
        }
        public bool CreateItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedItem = (User)selectedItem;
        }

        public List<object> LoadData()
        {
            List<User> userList = new List<User>();

            userList.Add(new User()
            {
                username = "admin",
                password = "admin"
            });
            userList.Add(new User()
            {
                username = "user",
                password = "user"
            });

            return userList.Cast<object>().ToList();

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
