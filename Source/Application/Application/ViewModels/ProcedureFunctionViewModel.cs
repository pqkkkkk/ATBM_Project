using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.TableView;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ViewModels
{
    public class ProcedureFunctionViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public IPrivilegeDao privilegeDao;
        public string objectType = "Procedure";
        public ObservableCollection<OracleObject> itemList { get; set; }
        public OracleObject? selectedObject { get; set; }

        public ProcedureFunctionViewModel()
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            privilegeDao = serviceProvider?.GetService<IPrivilegeDao>();
            itemList = new ObservableCollection<OracleObject>(LoadData().Cast<OracleObject>());
            selectedObject = null;
        }
        public int CreateItem(object item)
        {
            throw new NotImplementedException();
        }

        public int DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public List<object> LoadData()
        {
            List<Model.OracleObject> objects = privilegeDao.GetAllInstanceOfSpecificObject(objectType);


            return objects.Cast<object>().ToList();
        }

        public int UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            throw new NotImplementedException();
        }
        public List<string> GetSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            return itemList
                .Where(d => d.objectName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.objectName)
                .ToList();
        }
        public void search(string query)
        {
            if (query != "")
            {
                itemList = new ObservableCollection<OracleObject>(itemList.Where(item => item.objectName.ToLower().Contains(query)).ToList());
            }
            else
            {
                itemList = new ObservableCollection<OracleObject>(LoadData().Cast<OracleObject>());
            }
        }

    }
}
