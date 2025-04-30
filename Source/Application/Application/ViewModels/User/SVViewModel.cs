using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public SVViewModel()
        {
            selectedTabView = "DangKy";

            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKySVDao());
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonSVDao());
            daoList.Add("NhanVien", new NhanVienSVDao());
            daoList.Add("SinhVien", new SinhVienSVDao());

            dangKyList = new ObservableCollection<Model.DangKy>();
            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>();
            nhanVienList = new ObservableCollection<Model.NhanVien>();
            sinhVienList = new ObservableCollection<Model.SinhVien>();
        }
        public int DeleteItem()
        { 
            return 0;
        }
        public int UpdateItem()
        {
            return 0;
        }
        public int AddItem()
        {
            try
            {
                switch (selectedTabView)
                {
                    case "DangKy":
                        dangKyList.Add(new Model.DangKy()
                        {
                            maSV = "SV001",
                            maMM = "MM001",
                            diemTH = 10,
                            diemCT = 9,
                            diemCK = 8,
                            diemTK = 9
                        });
                        break;
                    case "SinhVien":
                        sinhVienList.Add(new Model.SinhVien()
                        {
                            maSV = "SV001",
                            hoTen = "Nguyen Van A",
                            phai = "Nam",
                            ngSinh = new DateOnly(2000, 1, 1),
                            dChi = "Ha Noi",
                            dt = "0123456789",
                            khoa = "CNTT",
                            tinhTrang = "Khoa"
                        });
                        break;
                    default:
                        break;
                }

                return 1;
            }
            catch (Exception ex)
            {
                // Handle exception
                return -1;
            }
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
