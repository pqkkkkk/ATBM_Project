using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.Role;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;


namespace Application.ViewModels
{    public class RoleDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public IRoleDao roleDao;
        public ObservableCollection<Role> itemList { get; set; }
        public Role? selectedRole { get; set; }

        public RoleDataViewModel()
        {
            var app = (App)App.Current;
            var connection = app.serviceProvider.GetRequiredService<OracleConnection>();
            roleDao = new RoleOracleDao(connection);
            itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
            selectedRole = null;
        }
        public bool CreateItem(object item)
        {
            if((item is string roleName && !string.IsNullOrEmpty(roleName)) && roleDao.CreateRole(roleName))
            {
                itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
                return true;
            }
            return false;
        }

        public int DeleteItem(object item)
        {
            if (roleDao.DropRole(selectedRole.name))
            {
                itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
                return true;
            }
            return false;
        }

        public bool UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedRole = (Role)selectedItem;
        }

        public List<object> LoadData()
        {
            List<string> roleList = roleDao.GetAllRoles();
            List<object> list = new List<object>();
            foreach (string roleName in roleList)
            {
                Role newRole = new Role()
                {
                    name = roleName
                };
                list.Add(newRole);
            }

            return list;
        }

        public List<string> GetSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            return itemList
                .Where(d => d.name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.name)
                .ToList();
        }

        public void search(string query)
        {
            if(query!= "")
            {
                itemList = new ObservableCollection<Role>(itemList.Where(item => item.name.ToLower().Contains(query)).ToList());
            }
            else
            {
                itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
