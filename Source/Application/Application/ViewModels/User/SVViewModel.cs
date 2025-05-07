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
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels.User
{
    public class SVViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        private Dictionary<string, object> newItemList;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private readonly Dictionary<string, IList> listMap;
        public SVViewModel()
        {
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;


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

            editableColumnMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { "maSV", "maMM"} },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { "maHP", "tenHP" } },
                { "MoMon", new List<string> { } },
                { "NhanVien", new List<string> { } },
                { "SinhVien", new List<string> { "dChi", "dt" } }
            };

            newItemList = new Dictionary<string, object>
            {
                { "DangKy", new Model.DangKy(){
                    isInDB = false
                    }
                },
                { "DonVi", new Model.DonVi() },
                { "HocPhan", new Model.HocPhan() },
                { "MoMon", new Model.MoMon() },
                { "NhanVien", new Model.NhanVien() },
                {
                "SinhVien", new Model.SinhVien()
                    {
                        isInDB = false,
                    }
                }
            };
            permissionMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { "insert","update", "select"} },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> {"select" } },
                { "NhanVien", new List<string> { } },
                { "SinhVien", new List<string> {"select", "update" } }
            };
        }
        public int DeleteItem()
        { 
            return 0;
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }

        }
        public int AddItem()
        {
            if(permissionMap.TryGetValue(selectedTabView, out var permissionList))
            {
                if (permissionList.Contains("insert") == false)
                {
                    return 0;
                }
            }

            if (listMap.TryGetValue(selectedTabView, out var list)
              && newItemList.TryGetValue(selectedTabView, out var newItem))
            {
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
            if (editableColumnMap.TryGetValue(selectedTabView, out var list))
            {
                return list.Contains(columnName);
            }

            return false;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
