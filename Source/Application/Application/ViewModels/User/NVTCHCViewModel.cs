using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.Role;
using Application.DataAccess.NhanVien;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels.User
{
    public class NVTCHCViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string selectedTabView { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private Dictionary<string, object> newItemList;
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }

        public NVTCHCViewModel()
        {
            selectedTabView = "DangKy";

            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            var sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;
            daoList = new Dictionary<string, IBaseDao>();
            daoList?.Add("NhanVien", new NhanVienNVTCHCDao(sqlConnection));

            nhanVienList = new ObservableCollection<NhanVien>();
            LoadData();

            listMap = new Dictionary<string, IList>
            {
                { "NhanVien", nhanVienList },
            };
            editableColumnMap = new Dictionary<string, IList>
            {
                { "NhanVien", new List<string> { "maNV","hoTen", "phai", "ngSinh", "luong", "phuCap", "dt", "vaiTro", "maDV" } },
            };
            permissionMap = new Dictionary<string, IList>
            {
                { "NhanVien", new List<string> { "select", "insert", "update", "delete" } },
            };

            newItemList = new Dictionary<string, object>();
            newItemList.Add("NhanVien", new Model.NhanVien()
            {
                isInDB = false,
            });
        }
        public int DeleteItem(object item)
        {
            if (item is IPersistable e)
            {
                if (e.isInDB == true)
                {
                    if (daoList["NhanVien"].Delete(item))
                    {
                        nhanVienList.Remove((Model.NhanVien)item);
                        return 1;
                    }
                }
            }
            return 0;
        }

        public void LoadData()
        {
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NhanVien"].Load(null).Cast<Model.NhanVien>().ToList());
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

        public void resetNewItem()
        {
            newItemList[selectedTabView] = new Model.NhanVien()
            {
                isInDB = false,
            };
        }
    }
}
