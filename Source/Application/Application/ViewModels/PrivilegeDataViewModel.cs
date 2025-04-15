using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.Role;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels
{
    public class PrivilegeDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IPrivilegeDao privilegeDao;
        private IRoleDao roleDao;
        public Model.CommonInfo selectedUserOrRole { get; set; }
        public string? selectedObjectType { get; set; }
        public ObservableCollection<Model.Role> roleList { get; set; }
        public ObservableCollection<Model.OracleObject> tableList { get; set; }
        public ObservableCollection<Model.OracleObject> viewList { get; set; }
        public ObservableCollection<Model.OracleObject> procedureList { get; set; }
        public ObservableCollection<Model.OracleObject> functionList { get; set; }

        public Model.OracleObject selectedTable { get; set; }
        public ObservableCollection<Model.ColumnOfObject> columnListOfSelectedObject { get; set; }
        public string? selectedActionOnTableOrView { get; set; }
        public bool canSelectColumnsOfTableOrView { get; set; }

        public Model.Role selectedRole { get; set; }
        public ObservableCollection<Model.Role> roleOfUsers { get; set; }
        public bool hasSelectedRole { get; set; }

        public ObservableCollection<Model.Privilege> itemList { get; set; }
        public Model.Privilege selectedItem { get; set; }
        public bool hasSelectedItem { get; set; }
        public PrivilegeDataViewModel(Model.CommonInfo selectedUserOrRole)
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            privilegeDao = serviceProvider?.GetService<IPrivilegeDao>();
            roleDao = serviceProvider?.GetService<IRoleDao>();

            selectedObjectType = "Table";
            this.selectedUserOrRole = selectedUserOrRole;

            roleList = new ObservableCollection<Model.Role>(roleDao.GetAllRolesWithRoleClass());
            tableList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Table"));
            viewList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("View"));
            procedureList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Procedure"));
            functionList = new ObservableCollection<Model.OracleObject>(privilegeDao.GetAllInstanceOfSpecificObject("Function"));

            itemList = new ObservableCollection<Model.Privilege>(privilegeDao.GetPrivilegesOfUserOnSpecificObjectType(selectedUserOrRole.name,selectedObjectType));
            selectedItem = new Model.Privilege();
            hasSelectedItem = false;

            selectedTable = new Model.OracleObject();
            columnListOfSelectedObject = new ObservableCollection<Model.ColumnOfObject>();
            selectedActionOnTableOrView = "";
            canSelectColumnsOfTableOrView = false;

            selectedRole = new Model.Role()
            {
                name = ""
            };
            hasSelectedRole = false;
            roleOfUsers = new ObservableCollection<Model.Role>(privilegeDao.GetAllRolesOfUser(selectedUserOrRole.name));

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
            selectedObjectType = objectType;
            itemList = new ObservableCollection<Model.Privilege>(LoadData().Cast<Model.Privilege>());
        }
        public List<object> LoadData()
        {
            if (selectedObjectType == "System privilege")
            {
                return privilegeDao.GetSystemPrivilegesOfUser(selectedUserOrRole.name).Cast<object>().ToList();
            }
            else if (selectedObjectType == "Column privilege")
            {
                return privilegeDao.GetColumnPrivilegesOfUser(selectedUserOrRole.name).Cast<object>().ToList();
            }
            else
            {
                return privilegeDao.GetPrivilegesOfUserOnSpecificObjectType(selectedUserOrRole.name, selectedObjectType).Cast<object>().ToList();
            }
        }
        public void UpdateSelectedObjectWhenSelectionChange(Model.OracleObject selectedItem)
        {
            if (selectedItem is Model.OracleObject oracleObject)
            {
                if (selectedObjectType == "Table" || selectedObjectType == "View")
                {
                    selectedTable = oracleObject;
                    columnListOfSelectedObject = new ObservableCollection<Model.ColumnOfObject>(privilegeDao.GetColumns(oracleObject.objectName));
                }
                else
                {
                    selectedTable = oracleObject;
                }
            }
        }
        public void UpdateSelectedRole(Model.Role selectedRole)
        {
            this.selectedRole = selectedRole;
            hasSelectedRole = true;
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

        public int DeleteItem(object item)
        {
            try
            {
                if (selectedItem.type == "")
                    throw new Exception("No item selected");

                if (selectedItem.type == "systemprivilege")
                    privilegeDao.RevokeSystemPrivilegesFromUser(selectedUserOrRole.name, selectedItem.privilege);
                else if (selectedItem.type == "columnprivilege")
                    privilegeDao.RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(selectedUserOrRole.name, selectedItem.privilege, selectedItem.tableName, selectedItem.columnName);
                else
                    privilegeDao.RevokePrivilegesOfUserOnSpecificObjectType(selectedUserOrRole.name, selectedItem.privilege, selectedItem.tableName);

                return 1;
            }
            catch (Exception e)
            {
                if (e.Message == "No item selected")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
        }
        public void GrantRole(string roleName, bool isWithGrantOption)
        {
            try
            {
                string withGrantOption = isWithGrantOption ? "YES" : "NO";
                string username = selectedUserOrRole.name;
                roleDao.GrantRole(username, roleName,withGrantOption);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public int RevokeRole()
        {
            try
            {
                if(selectedRole.name == "")
                {
                    throw new Exception("No role selected");
                }

                string username = selectedUserOrRole.name;
                string roleName = selectedRole.name;
                roleDao.RevokeRoleFromUser(username, roleName);

                return 1;
            }
            catch (Exception e)
            {
                if(e.Message == "No role selected")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
        }
        public int GrantPrivilegeOnProcedure(string? procedureName, bool isWithGrantOption)
        {
            string withGrantOption = isWithGrantOption == true ? "YES" : "NO";
            try
            {
                if (procedureName == "" || procedureName == null)
                {
                    throw new Exception("No procedure selected");
                }

                string username = selectedUserOrRole.name;
                privilegeDao.GrantPrivileges(username, "EXECUTE", procedureName, withGrantOption);
                return 1;
            }
            catch (Exception e)
            {
                if (e.Message == "No procedure selected")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
        }
        public int GrantPrivilegeOnTableOrView(string? privilege, string? objectName, bool isWithGrantOption)
        {
            try
            {
                if (objectName == "" || objectName == null)
                {
                    throw new Exception("No table or view selected");
                }
                if (privilege == "" || privilege == null)
                {
                    throw new Exception("No privilege selected");
                }

                string username = selectedUserOrRole.name;
                string withGrantOption = isWithGrantOption == true ? "YES" : "NO";
               
                if (privilege == "SELECT" || privilege == "UPDATE")
                {
                    string columns = "";
                    
                    foreach (var column in columnListOfSelectedObject)
                    {
                        if (column.isSelected)
                        {
                            columns += column.columnName + ",";
                        }
                    }
                    columns = columns.TrimEnd(',');
                    
                    if (columns == "")
                    {
                        throw new Exception("No column selected");
                    }
                    privilegeDao.GrantPrivilegesOnSpecificColumnsOfTableOrView(username, privilege, objectName, columns, withGrantOption);
                }
                else
                    privilegeDao.GrantPrivileges(username, privilege, objectName, withGrantOption);

                return 1;
            }
            catch (Exception e)
            {
                if (e.Message == "No table or view selected" || e.Message == "No privilege selected" || e.Message == "No column selected")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
            }
        }
        public void LoadRoleOfSelectedUser()
        {
            try
            {
                roleOfUsers = new ObservableCollection<Model.Role>(privilegeDao.GetAllRolesOfUser(selectedUserOrRole.name));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void UpdateSelectedActionOnTableOrView(string action)
        {
            selectedActionOnTableOrView = action;

            if (action == "SELECT" || action == "UPDATE")
            {
                canSelectColumnsOfTableOrView = true;
            }
            else
            {
                canSelectColumnsOfTableOrView = false;
            }
        }
        public void UpdateWhenCloseGrantPrivilegeOnTableOrViewDialog()
        {
            selectedActionOnTableOrView = "";
            canSelectColumnsOfTableOrView = false;
            selectedTable = new Model.OracleObject();
            columnListOfSelectedObject = new ObservableCollection<Model.ColumnOfObject>();
        }
        public void UpdateSelectedItem(Model.Privilege privilege)
        {
            selectedItem = privilege;
            hasSelectedItem = true;
            Debug.WriteLine("Updated selected item");
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
