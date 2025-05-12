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
using System.Collections;

namespace Application.ViewModels.User
{
    public class GVViewModel : INotifyPropertyChanged
    {
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.DonVi> donViList { get; set; }
        public ObservableCollection<Model.HocPhan> hocPhanList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }

        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;

        public GVViewModel()
        {
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;


            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKyGVDao(sqlConnection));
            daoList.Add("DonVi", new DonViSVDao());
            daoList.Add("HocPhan", new HocPhanSVDao());
            daoList.Add("MoMon", new MoMonGVDao(sqlConnection));
            daoList.Add("NhanVien", new NhanVienNVCBDao(sqlConnection));
            daoList.Add("SinhVien", new SinhVienGVDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DangKy"].Load(null).Cast<Model.DangKy>().ToList());

            donViList = new ObservableCollection<Model.DonVi>();
            hocPhanList = new ObservableCollection<Model.HocPhan>();
            moMonList = new ObservableCollection<Model.MoMon>(daoList["MoMon"].Load(null).Cast<Model.MoMon>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NhanVien"].Load(null).Cast<Model.NhanVien>().ToList());
            sinhVienList = new ObservableCollection<Model.SinhVien>(daoList["SinhVien"].Load(null).Cast<Model.SinhVien>().ToList());

            editableColumnMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> {} },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> { } },
                { "NhanVien", new List<string> {"dt" } },
                { "SinhVien", new List<string> { } }
            };
            permissionMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { "select"} },
                { "DonVi", new List<string> { } },
                { "HocPhan", new List<string> { } },
                { "MoMon", new List<string> {"select" } },
                { "NhanVien", new List<string> {"select", "update" } },
                { "SinhVien", new List<string> {"select" } }
            };
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
            return 1;
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
