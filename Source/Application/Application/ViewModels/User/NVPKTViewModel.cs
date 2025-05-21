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
    public class NVPKTViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private Dictionary<string, Func<object>> newItemFactoryMap;

        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public NVPKTViewModel()
        {
            selectedTabView = "DangKy";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DangKy", new DangKyNVPKTDao(sqlConnection));
            daoList.Add("NhanVien", new NhanVienNVCBDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DangKy"].Load(null).Cast<Model.DangKy>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NhanVien"].Load(null).Cast<Model.NhanVien>().ToList());

            listMap = new Dictionary<string, IList>
            {
                { "DangKy", dangKyList },
            };

            editableColumnMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { "diemTH", "diemCT", "diemCK", "diemTK" } },
            };

            permissionMap = new Dictionary<string, IList>
            {
                { "DangKy", new List<string> { "select", "update"} },
            };
            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DangKy"] = () => new Model.DangKy { isInDB = false },
                ["DonVi"] = () => new Model.DonVi(),
                ["HocPhan"] = () => new Model.HocPhan(),
                ["MoMon"] = () => new Model.MoMon(),
                ["NhanVien"] = () => new Model.NhanVien(),
                ["SinhVien"] = () => new Model.SinhVien { isInDB = false },
            };

        }
        public int SaveItem(object item)
        {
            try
            {
                if (item is IPersistable e)
                {
                    if (e.isInDB == true)
                    {
                        daoList[selectedTabView].Update(item);
                    }
                    else
                    {
                        daoList[selectedTabView].Add(item);
                        e.isInDB = true;
                    }
                }

                return 1;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }

        }
        public bool CheckTheColumnOfRowIsEditable(string columnName)
        {
            if (editableColumnMap.TryGetValue(selectedTabView, out var list))
            {
                return list.Contains(columnName);
            }

            return false;
        }
        public void UpdateSelectedTabView(string selectedTabView)
        {
            this.selectedTabView = selectedTabView;
        }
    }
}
