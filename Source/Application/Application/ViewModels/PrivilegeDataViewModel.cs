using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.Role;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels
{
    public class PrivilegeDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IPrivilegeDao privilegeDao;
        private IRoleDao roleDao;
        public Model.CommonInfo selectedItem { get; set; }
        public string? selectObjectType { get; set; }
        public ObservableCollection<Model.Role> roleList { get; set; }
        public ObservableCollection<Model.OracleObject> tableList { get; set; }
        public ObservableCollection<Model.OracleObject> viewList { get; set; }
        public ObservableCollection<Model.OracleObject> procedureList { get; set; }
        public ObservableCollection<Model.OracleObject> functionList { get; set; }

        public Model.OracleObject selectedObject { get; set; }
        public ObservableCollection<Model.ColumnOfObject>columnListOfSelectedObject { get; set; }
        public ObservableCollection<Model.TableAction> tableActions { get; set; }
        public Model.Role selectedRole { get; set; }
        public ObservableCollection<Model.Role> roleOfUsers { get; set; }
        public ObservableCollection<Model.Privilege> itemList { get; set; }
        public PrivilegeDataViewModel(Model.CommonInfo selectedItem)
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            privilegeDao = serviceProvider?.GetService<IPrivilegeDao>();
            roleDao = serviceProvider?.GetService<IRoleDao>();

            selectObjectType = "Table";
            this.selectedItem = selectedItem;

            roleList = new ObservableCollection<Model.Role>(roleDao.GetAllRolesWithRoleClass());
            tableList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Table"));
            viewList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("View"));
            procedureList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Procedure"));
            functionList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Function"));
            roleOfUsers = new ObservableCollection<Model.Role>(privilegeDao.GetAllRolesOfUser(selectedItem.name));
            itemList = new ObservableCollection<Model.Privilege>(privilegeDao.GetPrivilegesOfUserOnSpecificObjectType(selectedItem.name,selectObjectType));

            selectedObject = new Model.OracleObject();
            columnListOfSelectedObject = new ObservableCollection<Model.ColumnOfObject>();

            selectedRole = new Model.Role();

            tableActions = new ObservableCollection<Model.TableAction>()
            {
                new Model.TableAction()
                {
                    actionName = "SELECT",
                    isSelected = false
                },
                new Model.TableAction()
                {
                    actionName = "UPDATE",
                    isSelected = false
                },
                new Model.TableAction()
                {
                    actionName = "INSERT",
                    isSelected = false
                },
                new Model.TableAction()
                {
                    actionName = "DELETE",
                    isSelected = false
                }
            };

        }
        public List<Model.Role> LoadAllRolesOfUser(string username)
        {
            List<Model.Role> roles = new List<Model.Role>();
            try
            {
                roles = privilegeDao.GetAllRolesOfUser(username);
                return roles;
            }
            catch (Exception e)
            {
                return roles;
            }
        }
        public void UpdateSelectedObjectType(string objectType)
        {
            selectObjectType = objectType;
            itemList = new ObservableCollection<Model.Privilege>(LoadData().Cast<Model.Privilege>());
        }
        public List<object> LoadData()
        {
            if (selectObjectType == "System privilege")
            {
                return privilegeDao.GetSystemPrivilegesOfUser(selectedItem.name).Cast<object>().ToList();
            }
            else
            {
                return privilegeDao.GetPrivilegesOfUserOnSpecificObjectType(selectedItem.name, selectObjectType).Cast<object>().ToList();
            }
        }
        public void UpdateSelectedItemWhenSelectionChange(Model.OracleObject selectedItem)
        {
            if (selectedItem is Model.OracleObject oracleObject)
            {
                if (selectObjectType == "Table" || selectObjectType == "View")
                {
                    selectedObject = oracleObject;
                    columnListOfSelectedObject = new ObservableCollection<Model.ColumnOfObject>(privilegeDao.GetColumns(oracleObject.objectName));
                }
                else
                {
                    selectedObject = oracleObject;
                }
            }
        }
        public void UpdateSelectedRole(Model.Role selectedRole)
        {
            this.selectedRole = selectedRole;
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
