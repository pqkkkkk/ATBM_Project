using Application.DataAccess.DangKy;
using Application.DataAccess.DonVi;
using Application.DataAccess.HocPhan;
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;
using Application.DataAccess;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;
using Microsoft.UI.Xaml.Controls.Primitives;
using ABI.Windows.ApplicationModel.Activation;
using System.Collections;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using Application.Helper;
using Application.DataAccess.ThongBao;

namespace Application.ViewModels.User
{
    public class NVPDTViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public ObservableCollection<Model.OracleObject> tabViewList { get; set; }

        private Helper.Helper helper;
        private IPrivilegeDao privilegeDao;
        private ITableViewDao tableViewDao;
        public Dictionary<string, IBaseDao> daoList { get; set; }

        private readonly Dictionary<string, IList> listMap;
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }
        public ObservableCollection<Model.ThongBao> thongbaoList { get; set; }

        public Dictionary<string, Func<object>> newItemFactoryMap { get; set; }

        private readonly Dictionary<string, IList> editableColumnMap = new Dictionary<string, IList>();

        private readonly Dictionary<string, IList> permissionMap = new Dictionary<string, IList>();

        public NVPDTViewModel()
        {
            helper = new Helper.Helper();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;
            tabViewList = new ObservableCollection<Model.OracleObject>(tableList);
            selectedTabView = "DANGKY";

            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;

            privilegeDao = new PrivilegeUserDao(sqlConnection);
            tableViewDao = new TableViewUserDao(sqlConnection);

            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DANGKY", new DangKyNVPDTDao(sqlConnection));
            daoList.Add("DONVI", new DonViSVDao());
            daoList.Add("HOCPHAN", new HocPhanSVDao());
            daoList.Add("MOMON", new MoMonNVPDTDao(sqlConnection));
            daoList.Add("NHANVIEN", new NhanVienNVCBDao(sqlConnection));
            daoList.Add("SINHVIEN", new SinhVienNVPDTDao(sqlConnection));
            daoList.Add("THONGBAO", new ThongBaoXAdminDao(sqlConnection));


            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DANGKY"].Load(null).Cast<Model.DangKy>().ToList());
            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>(daoList["MOMON"].Load(null).Cast<Model.MoMon>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NHANVIEN"].Load(null).Cast<Model.NhanVien>().ToList());
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SINHVIEN"].Load(null).Cast<Model.SinhVien>().ToList());
            thongbaoList = new ObservableCollection<Model.ThongBao>(daoList["THONGBAO"].Load(null).Cast<Model.ThongBao>());

            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DANGKY"] = () => new Model.DangKy { isInDB = false },
                ["DONVI"] = () => new Model.DonVi(),
                ["HOCPHAN"] = () => new Model.HocPhan(),
                ["MOMON"] = () => new Model.MoMon(),
                ["NHANVIEN"] = () => new Model.NhanVien(),
                ["SINHVIEN"] = () => new Model.SinhVien { isInDB = false },
            };

            listMap = new Dictionary<string, IList>
            {
                {  "DANGKY", dangKyList },
                { "DONVI", donViList },
                { "HOCPHAN", hocPhanList },
                { "MOMON", moMonList },
                { "NHANVIEN", nhanVienList },
                {"SINHVIEN", sinhVienList},
                {"THONGBAO", thongbaoList}

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

        public void LoadPrivilegeOfRole()
        {

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return;

            foreach (var table in tableList)
            {
                permissionMap.Add(table.objectName.ToUpper(), new List<string> { });
                editableColumnMap.Add(table.objectName.ToUpper(), new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_GV", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName.ToUpper();
                if (permissionMap.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

                if (editableColumnMap.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != null)
                            columnList.Add(privilege.columnName);
                    }
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName;
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView).ToUpper();
                if (tableName.Contains("X_ADMIN"))
                {
                    tableName = tableName.Replace("X_ADMIN.", "");
                }

                if (permissionMap.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

                if (editableColumnMap.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != null)
                            columnList.Add(privilege.columnName);
                    }
                }

            }
            return;
        }
        public int DeleteItem(object item)
        {
            if (daoList[selectedTabView.ToUpper()].Delete(item))
            {
                if (listMap.TryGetValue(selectedTabView.ToUpper(), out var list))
                {
                    list.Remove(item);
                }
                return 1;
            }
            return 0;
        }
        public int UpdateItem()
        {
            return 0;
        }
        public int SaveItem(object obj)
        {
            try
            {
                if (obj is IPersistable item)
                {
                    if (item.isInDB == true)
                    {
                        if (daoList[selectedTabView.ToUpper()].Update(item))
                        {
                            moMonList = new ObservableCollection<Model.MoMon>(daoList["MOMON"].Load(null).Cast<Model.MoMon>().ToList());
                            return 1;
                        }
                    }
                    else
                    {
                        if (daoList[selectedTabView.ToUpper()].Add(item))
                        {
                            item.isInDB = true;
                            return 1;
                        }
                        else
                        {
                            if (listMap.TryGetValue(selectedTabView.ToUpper(), out var list))
                            {
                                list.Remove(item);
                            }
                            throw new System.Exception("Add failed");
                        }
                    }
                }

                return 1;
            }
            catch (System.Exception ex)
            {
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
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView.ToUpper();
        }
        public bool CheckTheColumnOfRowIsEditable(string columnName)
        {
            if (editableColumnMap.TryGetValue(selectedTabView.ToUpper(), out var list))
            {
                return list.Contains(columnName.ToUpper());
            }

            return false;
        }
        private void updateItemList(Dictionary<string, object> itemList)
        {
            itemList["DangKy"] = new Model.DangKy()
            {
                isInDB = false
            };
            itemList["DonVi"] = new Model.DonVi();
            itemList["HocPhan"] = new Model.HocPhan();
            itemList["MoMon"] = new Model.MoMon();
            itemList["NhanVien"] = new Model.NhanVien();
            itemList["SinhVien"] = new Model.SinhVien();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
