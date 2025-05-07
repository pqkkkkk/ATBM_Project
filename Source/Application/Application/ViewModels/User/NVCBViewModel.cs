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

namespace Application.ViewModels.User
{
    public class NVCBViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }

        public Dictionary<string, IBaseDao> daoList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        public NVCBViewModel()
        {
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;


            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao());
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao());
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
