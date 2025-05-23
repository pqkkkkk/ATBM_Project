using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DataAccess.DangKy;
using Application.DataAccess.DonVi;
using Application.DataAccess.HocPhan;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using Application.DataAccess.MoMon;
using Application.DataAccess.NhanVien;
using Application.DataAccess.SinhVien;
using Application.Helper;
using Application.Model;
using Oracle.ManagedDataAccess.Client;

namespace Application.ViewModels.User
{
    public class NVPKTViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Helper.Helper helper;
        private readonly OracleConnection sqlConnection;
        public string selectedTabView { get; set; }
        public IPrivilegeDao privilegeDao { get; set; }
        public ITableViewDao tableViewDao { get; set; }
        public Dictionary<string, IBaseDao> daoList { get; set; }
        private readonly Dictionary<string, IList> listMap;
        private readonly Dictionary<string, IList> editableColumnMap;
        private readonly Dictionary<string, IList> permissionMap;
        private Dictionary<string, Func<object>> newItemFactoryMap;
      
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public NVPKTViewModel()
        {
            helper = new Helper.Helper();

            selectedTabView = "DANGKY";
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;
            sqlConnection = serviceProvider?.GetService(typeof(OracleConnection)) as OracleConnection;
            tableViewDao = new TableViewUserDao(sqlConnection);
            privilegeDao = new PrivilegeUserDao(sqlConnection);
            daoList = new Dictionary<string, IBaseDao>();
            daoList.Add("DANGKY", new DangKyNVPKTDao(sqlConnection));
            daoList.Add("NHANVIEN", new NhanVienNVCBDao(sqlConnection));

            dangKyList = new ObservableCollection<Model.DangKy>(daoList["DANGKY"].Load(null).Cast<Model.DangKy>().ToList());
            nhanVienList = new ObservableCollection<Model.NhanVien>(daoList["NHANVIEN"].Load(null).Cast<Model.NhanVien>().ToList());

            listMap = new Dictionary<string, IList>
            {
                { "DANGKY", dangKyList },
            };

            editableColumnMap = new Dictionary<string, IList>(LoadEditableColumnsOfUser());

            permissionMap = new Dictionary<string, IList>(LoadPrivilegesOfUser());
            newItemFactoryMap = new Dictionary<string, Func<object>>
            {
                ["DANGKY"] = () => new Model.DangKy { isInDB = false },
                ["DONVI"] = () => new Model.DonVi(),
                ["HOCPHAN"] = () => new Model.HocPhan(),
                ["MOMON"] = () => new Model.MoMon(),
                ["NHANVIEN"] = () => new Model.NhanVien(),
                ["SINHVIEN"] = () => new Model.SinhVien { isInDB = false },
            };

    }
        public Dictionary<string, IList> LoadPrivilegesOfUser()
        {
            Dictionary<string, IList> result = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return result;

            foreach (var table in tableList)
            {
                result.Add(table.objectName, new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVTCHC", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName;
                if (result.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName;
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView);
                if (tableName.Contains("X_ADMIN") == true)
                {
                    tableName = tableName.Replace("X_ADMIN.", "");
                }

                if (result.TryGetValue(tableName, out var permissionList))
                {
                    if (permissionList.Contains(privilege.privilege) == false)
                        permissionList.Add(privilege.privilege);
                }

            }
            return result;
        }
        public Dictionary<string, IList> LoadEditableColumnsOfUser()
        {
            Dictionary<string, IList> result = new Dictionary<string, IList>();

            var tableList = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;

            if (tableList == null)
                return result;

            foreach (var table in tableList)
            {
                result.Add(table.objectName, new List<string> { });
            }

            List<Model.Privilege> privileges = privilegeDao.GetPrivilegesOfUserOnSpecificObjectType("XR_NVPKT", "TABLE");

            foreach (var privilege in privileges)
            {
                string tableName = privilege.tableName;

                if (result.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != "")
                            columnList.Add(privilege.columnName);
                        else
                        {
                            List<string> columnListOfTable = tableViewDao.GetColumnListOfTableOrView(tableName);
                            foreach (var column in columnListOfTable)
                            {
                                if (columnList.Contains(column) == false)
                                    columnList.Add(column);
                            }
                        }
                    }
                }
            }
            foreach (var privilege in privileges)
            {
                string viewName = privilege.tableName;
                string? textOfView = tableViewDao.GetTextOfView(viewName);
                if (textOfView == null)
                    continue;

                string tableName = helper.GetTableNameFromTextOfView(textOfView);
                if (tableName.Contains("X_ADMIN") == true)
                {
                    tableName = tableName.Replace("X_ADMIN.", "");
                }

                if (result.TryGetValue(tableName, out var columnList))
                {
                    if (privilege.privilege == "UPDATE")
                    {
                        if (privilege.columnName != "")
                            columnList.Add(privilege.columnName);
                        else
                        {
                            List<string> columnListOfTable = tableViewDao.GetColumnListOfTableOrView(viewName);
                            foreach (var column in columnListOfTable)
                            {
                                if (columnList.Contains(column) == false)
                                    columnList.Add(column);
                            }
                        }
                    }
                }

            }
            return result;
        }
        public List<string> GetColumnListOfTableOrView(string tableName)
        {
            var result = new List<string>();
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                const string owner = "X_ADMIN";
                const string sql = @"SELECT COLUMN_NAME 
                             FROM ALL_TAB_COLUMNS 
                             WHERE OWNER = :owner AND TABLE_NAME = :tableName";

                using var cmd = new OracleCommand(sql, sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;
                cmd.Parameters.Add("tableName", OracleDbType.Varchar2).Value = tableName.ToUpper();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var columnName = reader["COLUMN_NAME"] as string;
                    if (!string.IsNullOrEmpty(columnName))
                        result.Add(columnName);
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message, e);
            }
            return result;
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
