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
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            roleDao = serviceProvider?.GetService<IRoleDao>();
            itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
            selectedRole = null;
        }
        public int CreateItem(object item)
        {
            try
            {
                string roleName = (string)item;
                if (string.IsNullOrEmpty(roleName))
                {
                    return (int)CreateRoleResult.InvalidRoleName;
                }
                if (roleDao.CheckExist("ROLE", roleName))
                {
                    return (int)CreateRoleResult.RoleAlreadyExists;
                }
                if (roleDao.CreateRole(roleName))
                {
                    itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
                    return (int)CreateRoleResult.Success;
                }
                else
                {
                    return (int)CreateRoleResult.RoleCreationFailed;
                }
            }
            catch (Exception e)
            {
                return (int)CreateRoleResult.UnknownError;
            }

        }

        public int DeleteItem(object item)
        {
            if (roleDao.DropRole(selectedRole.name))
            {
                itemList = new ObservableCollection<Role>(LoadData().Cast<Role>());
                return 1;
            }
            return 0;
        }

        public int UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            selectedRole = (Role)selectedItem;
        }

        public List<object> LoadData()
        {
            List<Model.Role> roleList = roleDao.GetAllRoles();
           

            return roleList.Cast<object>().ToList();
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

        int BaseViewModel.DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
