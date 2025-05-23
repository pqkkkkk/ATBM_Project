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
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using System.Collections;

namespace Application.ViewModels.User
{
    public class NVCBViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }

        private Helper.Helper helper;
        private IPrivilegeDao privilegeDao;
        private ITableViewDao tableViewDao;
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        private readonly Dictionary<string, IList> editableColumnMap = new Dictionary<string, IList>();

        private readonly Dictionary<string, IList> permissionMap = new Dictionary<string, IList>();

        private readonly Dictionary<string, IList> listMap;

        public NVCBViewModel()
        {
            helper = new Helper.Helper();

            selectedTabView = "DANGKY";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;

            privilegeDao = new PrivilegeUserDao(sqlConnection);
            tableViewDao = new TableViewUserDao(sqlConnection);
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DANGKY", new DangKySVDao(sqlConnection));
            daoList.Add("DONVI", new DonViSVDao());
            daoList.Add("HOCPHAN", new HocPhanSVDao());
            daoList.Add("NHANVIEN", new NhanVienNVCBDao(sqlConnection));
            daoList.Add("SINHVIEN", new SinhVienSVDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>();
           

            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>();
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NHANVIEN"].Load(null).Cast<Model.NhanVien>().ToList());
            sinhVienList = new ObservableCollection<Model.SinhVien>();

            LoadPrivilegeOfRole();
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

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVCB", "TABLE");

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
        public int DeleteItem()
        {
            return 0;
        }
        public int Update(object obj)
        {
            if (obj is IPersistable item)
            {
                if (item.isInDB == true)
                {
                    daoList[selectedTabView].Update(item);
                    return 1;
                }
            }
            return 0;
        }
        public int AddItem()
        {
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
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
