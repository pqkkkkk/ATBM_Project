using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.DangKy;
using Application.DataAccess.DonVi;
using Application.DataAccess.HocPhan;
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;
using Application.DataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System.Collections;
using Application.Model;
using Windows.Media.Devices;

namespace Application.ViewModels.User
{
    public class NVCTSVViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public Dictionary<string, object> newItemList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        public NVCTSVViewModel()
        {
            selectedTabView = "DangKy";

            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;

            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKyNVCTSVDao());
            daoList.Add("DonVi", new DonViNVCTSVDao());
            daoList.Add("HocPhan", new HocPhanNVCTSVDao());
            daoList.Add("MoMon", new MoMonNVCTSVDao());
            daoList.Add("NhanVien", new NhanVienNVCTSVDao());
            daoList.Add("SinhVien", new SinhVienNVCTSVDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>();
            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>();
            nhanVienList = new ObservableCollection<Model.NhanVien>();
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SinhVien"].Load(null).Cast<Model.SinhVien>().ToList());

            newItemList = new Dictionary<string, object>();
            newItemList.Add("DangKy", new Model.DangKy());
            newItemList.Add("DonVi", new Model.DonVi());
            newItemList.Add("HocPhan", new Model.HocPhan());
            newItemList.Add("MoMon", new Model.MoMon());
            newItemList.Add("NhanVien", new Model.NhanVien());
            newItemList.Add("SinhVien", new Model.SinhVien()
            {
                isInDB = false,
            });

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
                { "DangKy", new List<string> { } },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> { } },
                { "NhanVien", new List<string> { } },
                {"SinhVien", new List<string> {"maSV","hoTen","phai","ngSinh","dChi","dt","khoa"}}
            };
            permissionMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { } },
                { "DonVi", new List<string> {  } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> { } },
                { "NhanVien", new List<string> { } },
                {"SinhVien", new List<string> {"insert","update","delete","select"}}
            };
        }
        public int DeleteItem(object item)
        {
            try
            {
                var dao = daoList[selectedTabView];

                if (item is IPersistable e)
                {
                    if (e.isInDB == true)
                    {
                        dao.Delete(item);
                    }

                    var list = listMap[selectedTabView];
                    list.Remove(item);
                }
                else
                {
                    var list = listMap[selectedTabView];
                    list.Remove(item);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }
        public int SaveItem(object item)
        {
            try
            {
                var dao = daoList[selectedTabView];

                if(item is IPersistable e)
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
            if (permissionMap.TryGetValue(selectedTabView, out var permissionList))
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
        public bool CheckTheColumnOfRowIsEditable(string columnName)
        {
            if(editableColumnMap.TryGetValue(selectedTabView, out var list))
            {
                return list.Contains(columnName);
            }

            return false;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
