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
            itemList = new ObservableCollection<User>(LoadData().Cast<User>());
            selectedUser = null;
        }
        public bool CreateItem(object item)
        {
            var user = (User)item;
            if (user.username == null)
            {
                throw new ArgumentNullException(nameof(user.username), "Username cannot be null.");
            }
            if (user.password == null)
            {
                throw new ArgumentNullException(nameof(user.password), "Password cannot be null.");
            }

            var serviceProvider = ((App)App.Current).serviceProvider;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }

            var connection = serviceProvider.GetRequiredService<OracleConnection>();
            var userOracleDao = new UserOracleDao(connection);
            return userOracleDao.CreateUser(user.username, user.password);
        }

        public bool DeleteItem(object item)
        {
            var serviceProvider = ((App)App.Current).serviceProvider;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            var connection = serviceProvider.GetRequiredService<OracleConnection>();
            if (connection == null)
            {
                throw new InvalidOperationException("Oracle connection is not initialized.");
            }
            UserOracleDao userOracleDao = new UserOracleDao(connection);
            var user = (User)item;
            return userOracleDao.DeleteUser(user.username);
        }

        public bool UpdateItem(object item)
        {
            var serviceProvider = ((App)App.Current).serviceProvider;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }

            var connection = serviceProvider.GetRequiredService<OracleConnection>();
            if (connection == null)
            {
                throw new InvalidOperationException("Oracle connection is not initialized.");
            }
            UserOracleDao userOracleDao = new UserOracleDao(connection);
            var user = (User)item;
            return userOracleDao.UpdatePassword(user.username, user.password);
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedItem = (User)selectedItem;
        }

        public List<User> LoadData()
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            userDao = serviceProvider?.GetService<IUserDao>();

            var users = userDao.LoadData().Select(obj =>
            {
                dynamic x = obj;
                return new User
                {
                    username = x.Username,
                    password = x.Password
                };
            })
            .ToList();
            return users;
        }

        List<object> BaseViewModel.LoadData()
        {
            throw new NotImplementedException();
        }

        int BaseViewModel.DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
