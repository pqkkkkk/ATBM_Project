using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.User;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Automation.Peers;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels
{
    public class UserDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<User> itemList { get; set; }
        public User? selectedUser { get; set; }
        public IUserDao? userDao { get; set; }

        public UserDataViewModel()
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            userDao = serviceProvider?.GetService<IUserDao>();

            itemList = new ObservableCollection<User>(userDao.LoadData());
            selectedUser = null;
        }
        public int CreateItem(object item)
        {
            try
            {
                var user = (User)item;

                if (String.IsNullOrEmpty(user.username) || String.IsNullOrEmpty(user.password))
                {
                    return (int)CreateUserResult.InvalidUsernameOrPassword;
                }
                if (userDao.CheckExist("USER", user.username))
                {
                    return (int)CreateUserResult.UserAlreadyExists;
                }
                if (userDao.CreateUser(user.username, user.password))
                {
                    itemList = new ObservableCollection<User>(userDao.LoadData());
                    return (int)CreateUserResult.Success;
                }
                else
                {
                    return (int)CreateUserResult.UserCreationFailed;
                }
            }
            catch (Exception e)
            {
                return (int)CreateUserResult.UnknownError;
            }
        }

        public int DeleteItem(object item)
        {
            var user = (User)item;
            
            
            if(userDao.DeleteUser(user.username))
            {
                itemList.Remove(user);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int UpdateItem(object item)
        {
            try
            {
                if (selectedUser == null)
                {
                    return (int)UpdateUserResult.NoSelectedUser;
                }

                string newPassword = ((User)item).password;

                if (String.IsNullOrEmpty(newPassword))
                {
                    return (int)UpdateUserResult.InvalidPassword;
                }

                if (userDao.UpdatePassword(selectedUser.username, newPassword))
                {
                    itemList = new ObservableCollection<User>(userDao.LoadData());
                    return (int)UpdateUserResult.Success;
                }
                else
                {
                    return (int)UpdateUserResult.UserUpdateFailed;
                }
            }
            catch (Exception e)
            {
                return (int)UpdateUserResult.UnknownError;
            }
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedUser = (User)selectedItem;
        }

        List<object> BaseViewModel.LoadData()
        {
            throw new NotImplementedException();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
