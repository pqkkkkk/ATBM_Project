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

namespace Application.ViewModels.User
{
    public class NVPDTViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }

        private readonly Dictionary<string, IList> listMap;
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        public Dictionary<string, Func<object>> newItemFactoryMap { get; set; }
        public Dictionary<string, IList> editableColumnMap { get; set; }
        public Dictionary<string, IList> permissionMap { get; set; }

        public NVPDTViewModel()
        {
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKyNVPDTDao(sqlConnection));
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonNVPDTDao(sqlConnection));
            daoList.Add("NhanVien", new NhanVienNVCBDao(sqlConnection));
            daoList.Add("SinhVien", new SinhVienNVPDTDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DangKy"].Load(null).Cast<Model.DangKy>().ToList());
            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>(daoList["MoMon"].Load(null).Cast<Model.MoMon>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NhanVien"].Load(null).Cast<Model.NhanVien>().ToList());
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SinhVien"].Load(null).Cast<Model.SinhVien>().ToList());

            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DangKy"] = () => new Model.DangKy { isInDB = false },
                ["DonVi"] = () => new Model.DonVi(),
                ["HocPhan"] = () => new Model.HocPhan(),
                ["MoMon"] = () => new Model.MoMon(),
                ["NhanVien"] = () => new Model.NhanVien(),
                ["SinhVien"] = () => new Model.SinhVien { isInDB = false },
            };

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
                { "MoMon", new List<string> {"maMM","maHP","maGV","hk","nam" } },
                { "NhanVien", new List<string> {"dt"} },
                { "SinhVien", new List<string> { "tinhTrang" } }
            };
            permissionMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> {"delete", "insert","update", "select"} },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> {"select","update","insert","delete" } },
                { "NhanVien", new List<string> {"select","update" } },
                { "SinhVien", new List<string> {"select", "update" } }
            };
        }
        public int DeleteItem(object item)
        {
            if (daoList[selectedTabView].Delete(item)) 
                return 1;
            return 0;
        }
        public int UpdateItem()
        {
            return 0;
        }
        public int SaveItem(object obj)
        {
            if(obj is IPersistable item)
            {
                if(item.isInDB == true)
                {
                    if (daoList[selectedTabView].Update(item)) return 1;
                }
                else
                {
                    if(daoList[selectedTabView].Add(item))
                    {
                        item.isInDB = true;
                        return 1;
                    }
                }
            }
            return 0;
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
            if (editableColumnMap.TryGetValue(selectedTabView, out var list))
            {
                return list.Contains(columnName);
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
