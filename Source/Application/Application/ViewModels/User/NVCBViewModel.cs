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
<<<<<<< HEAD
=======
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using System.Collections;
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8

namespace Application.ViewModels.User
{
    public class NVCBViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }

<<<<<<< HEAD
=======
        private Helper.Helper helper;
        private IPrivilegeDao privilegeDao;
        private ITableViewDao tableViewDao;
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        public NVCBViewModel()
        {
<<<<<<< HEAD
=======
            helper = new Helper.Helper();

>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;

<<<<<<< HEAD

            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao());
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao());
=======
            privilegeDao = new PrivilegeUserDao(sqlConnection);
            tableViewDao = new TableViewUserDao(sqlConnection);
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao(sqlConnection));
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao(sqlConnection));
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
            daoList.Add("NhanVien", new NhanVienNVCBDao(sqlConnection));
            daoList.Add("SinhVien", new SinhVienSVDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>();
            dangKyList.Add(new Model.DangKy()
            {
                maSV = "SV001",
                maMM = "MM001",
                diemTH = 10,
                diemCT = 9,
                diemCK = 8,
                diemTK = 9
            });

            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>();
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NhanVien"].Load(null).Cast<Model.NhanVien>().ToList());
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SinhVien"].Load(null).Cast<Model.SinhVien>().ToList());
        }
<<<<<<< HEAD
=======
        public void LoadPrivilegeOfRole()
        {
            Dictionary<string, IList> editableColumnMapTest = new Dictionary<string, IList>();
            Dictionary<string, IList> permissionMapTest = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return;

            foreach (var table in tableList)
            {
                permissionMapTest.Add(table.objectName, new List<string> { });
                editableColumnMapTest.Add(table.objectName, new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVCB", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName;
                if (permissionMapTest.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

                if (editableColumnMapTest.TryGetValue(tableName, out var columnList))
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

                string tableName = helper.GetTableNameFromTextOfView(textOfView);

                if (permissionMapTest.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

                if (editableColumnMapTest.TryGetValue(tableName, out var columnList))
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
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8
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
            return 1;
        }


        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
