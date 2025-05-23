using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DataAccess.DangKy;
using Application.DataAccess.DonVi;
using Application.DataAccess.HocPhan;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels.User
{
    public class SVViewModel : INotifyPropertyChanged
    {
        private Helper.Helper helper;
        public string selectedTabView { get; set; }

        public IPrivilegeDao privilegeDao { get; set; }
        public ITableViewDao tableViewDao { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }

        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        private Dictionary<string, Func<object>> newItemFactoryMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private readonly Dictionary<string, IList> listMap;
        public SVViewModel()
        {
            helper = new Helper.Helper();

            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;

            tableViewDao = new TableViewUserDao(sqlConnection);
            privilegeDao = new PrivilegeUserDao(sqlConnection);
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao(sqlConnection));
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao(sqlConnection));
            daoList.Add("NhanVien", new NhanVienSVDao());
            daoList.Add("SinhVien", new SinhVienSVDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DangKy"].Load(null).Cast<Model.DangKy>().ToList());
            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>(daoList["MoMon"].Load(null).Cast<Model.MoMon>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>();
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SinhVien"].Load(null).Cast<Model.SinhVien>().ToList());

            listMap = new Dictionary<string, IList>
            {
                {  "DangKy", dangKyList },
                { "DonVi", donViList },
                { "HocPhan", hocPhanList },
                { "MoMon", moMonList },
                { "NhanVien", nhanVienList },
                {"SinhVien", sinhVienList}
            };

            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DangKy"] = () => new Model.DangKy { isInDB = false },
                ["DonVi"] = () => new Model.DonVi(),
                ["HocPhan"] = () => new Model.HocPhan(),
                ["MoMon"] = () => new Model.MoMon(),
                ["NhanVien"] = () => new Model.NhanVien(),
                ["SinhVien"] = () => new Model.SinhVien { isInDB = false },
            };

            permissionMap = LoadPrivilegesOfUser();
            editableColumnMap = LoadEditableColumnsOfUser();
        }
        public Dictionary<string, IList> LoadPrivilegesOfUser()
        {
            Dictionary<string, IList> result = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return result;

            foreach (var table in tableList)
            {
                result.Add(table.objectName, new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVTCHC", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName;
                if (result.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName;
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView);
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
                result.Add(table.objectName, new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVTCHC", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName;

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
                string viewName = privilege.tableName;
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView);
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

        public int SaveItem(object item)
        {
            try
            {
                var dao = daoList[selectedTabView];

                if (item is IPersistable e)
                {
                    if (e.isInDB == true)
                    {
                        dao.Update(item);
                    }
                    else
                    {
                        dao.Add(item);
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
        public int DeleteItem(object item)
        {
            try
            {
                var dao = daoList[selectedTabView];

                if (item is IPersistable e)
                {
                    bool deleteResult = false;
                    if (e.isInDB == true)
                    {
                        deleteResult = dao.Delete(item);
                    }

                    if (deleteResult)
                    {
                        var list = listMap[selectedTabView];
                        list.Remove(item);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    var list = listMap[selectedTabView];
                    list.Remove(item);
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
            if(permissionMap.TryGetValue(selectedTabView.ToUpper(), out var permissionList))
            {
                if (permissionList.Contains("insert".ToUpper()) == false)
                {
                    return 0;
                }
            }

            if (listMap.TryGetValue(selectedTabView, out var list)
              && newItemFactoryMap.TryGetValue(selectedTabView, out var factory))
            {
                var newItem = factory();
                list.Add(newItem);
                return 1;
            }

            return 0;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        public bool CheckTheColumnOfRowIsEditable(string columnName)
        {
            if (editableColumnMap.TryGetValue(selectedTabView.ToUpper(), out var list))
            {
                return list.Contains(columnName.ToUpper());
            }

            return false;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
