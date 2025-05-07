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
        public Dictionary<string, object> newItemList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

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
            daoList.Add("NhanVien", new NhanVienSVDao());
            daoList.Add("SinhVien", new SinhVienNVPDTDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DangKy"].Load(null).Cast<Model.DangKy>().ToList());

            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>(daoList["MoMon"].Load(null).Cast<Model.MoMon>().ToList());
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
            if (listMap.TryGetValue(selectedTabView, out var list)
                && newItemList.TryGetValue(selectedTabView, out var newItem))
            {
                list.Add(newItem);
                updateItemList(newItemList);
                return 1;
            }

            return 0;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        private void updateItemList(Dictionary<string, object> itemList)
        {
            itemList["DangKy"] = new Model.DangKy();
            itemList["DonVi"] = new Model.DonVi();
            itemList["HocPhan"] = new Model.HocPhan();
            itemList["MoMon"] = new Model.MoMon();
            itemList["NhanVien"] = new Model.NhanVien();
            itemList["SinhVien"] = new Model.SinhVien();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
