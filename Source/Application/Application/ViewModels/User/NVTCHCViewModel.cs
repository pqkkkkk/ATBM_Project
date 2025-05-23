using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.Role;
using Application.DataAccess.MetaData.TableView;
using Application.DataAccess.NhanVien;
using Application.Helper;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels.User
{
    public class NVTCHCViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Helper.Helper helper;
        private readonly OracleConnection sqlConnection;

        public string selectedTabView { get; set; }
        public ObservableCollection<Model.OracleObject> tabViewList { get; set; }

        public IPrivilegeDao privilegeDao { get; set; }
        public ITableViewDao tableViewDao { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private Dictionary<string, Func<object>> newItemFactoryMap;
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }

        public NVTCHCViewModel()
        {
            helper = new Helper.Helper();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;
            tabViewList = new ObservableCollection<Model.OracleObject>(tableList);
            selectedTabView = "DANGKY";

            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;
            daoList = new Dictionary<string, IBaseDao>();
            daoList?.Add("NHANVIEN", new NhanVienNVTCHCDao(sqlConnection));
            tableViewDao = new TableViewUserDao(sqlConnection);
            privilegeDao = new PrivilegeUserDao(sqlConnection);

            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NHANVIEN"].Load(null).Cast<Model.NhanVien>().ToList());

            listMap = new Dictionary<string, IList>
            {
                { "NHANVIEN", nhanVienList },
            };
            permissionMap = new Dictionary<string, IList>(LoadPrivilegesOfUser());
            editableColumnMap = new Dictionary<string, IList>(LoadEditableColumnsOfUser());

            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DANGKY"] = () => new Model.DangKy { isInDB = false },
                ["DONVI"] = () => new Model.DonVi(),
                ["HOCPHAN"] = () => new Model.HocPhan(),
                ["MOMON"] = () => new Model.MoMon(),
                ["NHANVIEN"] = () => new Model.NhanVien() { isInDB = false},
                ["SINHVIEN"] = () => new Model.SinhVien { isInDB = false },
            };
        }

        public Dictionary<string, IList> LoadPrivilegesOfUser()
        {
            Dictionary<string, IList> result = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return result;

            foreach (var table in tableList)
            {
                result.Add(table.objectName.ToUpper(), new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVTCHC", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName.ToUpper();
                if (result.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName.ToUpper();
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView).ToUpper();
                if (tableName.Contains("X_ADMIN") == true)
                {
                    tableName = tableName.Replace("X_ADMIN.", "");
                }

                if (result.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

            }
            return result;
        }
        public Dictionary<string, IList> LoadEditableColumnsOfUser()
        {
            Dictionary<string, IList> result = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return result;

            foreach (var table in tableList)
            {
                result.Add(table.objectName.ToUpper(), new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVTCHC", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName.ToUpper();

                if (result.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != "")
                            columnList.Add(privilege.columnName);
                        else
                        {
                            List<string> columnListOfTable = tableViewDao.GetColumnListOfTableOrView(tableName);
                            foreach (var column in columnListOfTable)
                            {
                                if (columnList.Contains(column) == false)
                                    columnList.Add(column);
                            }
                        }
                    }
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName.ToUpper();
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView).ToUpper();
                if (tableName.Contains("X_ADMIN") == true)
                {
                    tableName = tableName.Replace("X_ADMIN.", "");
                }

                if (result.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != "")
                            columnList.Add(privilege.columnName);
                        else
                        {
                            List<string> columnListOfTable = tableViewDao.GetColumnListOfTableOrView(viewName);
                            foreach (var column in columnListOfTable)
                            {
                                if (columnList.Contains(column) == false)
                                    columnList.Add(column);
                            }
                        }
                    }
                }

            }
            return result;
        }
        public int DeleteItem(object item)
        {
            if (item is IPersistable e)
            {
                if (e.isInDB == true)
                {
                    if (daoList["NHANVIEN"].Delete(item))
                    {
                        nhanVienList.Remove((Model.NhanVien)item);
                        return 1;
                    }
                }
            }
            return 0;
        }
        public int SaveItem(object item)
        {
            try
            {
                if (item is IPersistable e)
                {
                    if (e.isInDB == true)
                    {
                        
                        bool result =  daoList[selectedTabView].Update(item);

                        if(result == false)
                        {
                            throw new System.Exception("Update failed");
                        }
                    }
                    else
                    {
                        bool result =  daoList[selectedTabView].Add(item);
                        if (result == false)
                        {
                            if(listMap.TryGetValue(selectedTabView.ToUpper(), out var list))
                            {
                                list.Remove(item);
                            }
                            throw new System.Exception("Insert failed");
                        }
                        else
                            e.isInDB = true;
                    }
                }

                return 1;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }

        }
        public int AddItem()
        {
            if (permissionMap.TryGetValue(selectedTabView.ToUpper(), out var permissionList))
            {
                if (permissionList.Contains("INSERT") == false)
                {
                    return 0;
                }
            }

            if (listMap.TryGetValue(selectedTabView.ToUpper(), out var list)
                && newItemFactoryMap.TryGetValue(selectedTabView.ToUpper(), out var factory))
            {
                var newItem = factory();
                list.Add(newItem);
                return 1;
            }
            
            return 0;
        }
        public bool CheckTheColumnOfRowIsEditable(string columnName)
        {
            if (editableColumnMap.TryGetValue(selectedTabView.ToUpper(), out var list))
            {
                return list.Contains(columnName.ToUpper());
            }

            return false;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView.ToUpper();
        }
    }
}
