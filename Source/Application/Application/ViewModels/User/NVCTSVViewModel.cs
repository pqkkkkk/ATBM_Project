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

namespace Application.ViewModels.User
{
    public class NVCTSVViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public Dictionary<string, object> newItemList { get; set; }
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
            newItemList.Add("SinhVien", new Model.SinhVien());
        }
        public int DeleteItem()
        {
            return 0;
        }
        public int UpdateItem()
        {
            return 0;
        }
        public int SaveItem(object item)
        {
            try
            {
                bool? isInDB = false;
                if (selectedTabView == "SinhVien")
                {
                    var sv = item as Model.SinhVien;
                    isInDB = sv.GetIsInDB();
                }


                if (isInDB != null && isInDB == false)
                {
                    daoList[selectedTabView].Add(item);
                }
                else
                {
                    daoList[selectedTabView].Update(item);
                }

                if (selectedTabView == "SinhVien")
                {
                    var sv = item as Model.SinhVien;
                    sinhVienList.FirstOrDefault(x => x.maSV == sv.maSV).SetIsInDB(true);
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
            if (selectedTabView == "DangKy")
            {
                var item = newItemList["DangKy"] as Model.DangKy;
                if (item != null)
                {
                    dangKyList.Add(item);
                }
            }
            else if (selectedTabView == "DonVi")
            {
                var item = newItemList["DonVi"] as Model.DonVi;
                if (item != null)
                {
                    donViList.Add(item);
                }
            }
            else if (selectedTabView == "HocPhan")
            {
                var item = newItemList["HocPhan"] as Model.HocPhan;
                if (item != null)
                {
                    hocPhanList.Add(item);
                }
            }
            else if (selectedTabView == "MoMon")
            {
                var item = newItemList["MoMon"] as Model.MoMon;
                if (item != null)
                {
                    moMonList.Add(item);
                }
            }
            else if (selectedTabView == "NhanVien")
            {
                var item = newItemList["NhanVien"] as Model.NhanVien;
                if (item != null)
                {
                    nhanVienList.Add(item);
                }
            }
            else if (selectedTabView == "SinhVien")
            {
                var item = newItemList["SinhVien"] as Model.SinhVien;
                if (item != null)
                {
                    sinhVienList.Add(item);
                }
            }
            return 1;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
