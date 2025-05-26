using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData.Role;
using Application.DataAccess.ThongBao;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;

namespace Application.ViewModels
{
    public class NotificationDataViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public IThongBaoDao? TBDao { get; set; }
        public ObservableCollection<ThongBao> itemList { get; set; }
        public ObservableCollection<LabelComponent> levelList { get; set; }
        public ObservableCollection<LabelComponent> departmentList { get; set; }
        public ObservableCollection<LabelComponent> groupList { get; set; }
        public LabelComponent levelSelected { get; set; }
        public ObservableCollection<LabelComponent> selectedDepartments { get; set; }
        public ObservableCollection<LabelComponent> selectedGroups { get; set; }


        public NotificationDataViewModel()
        {
            var serviceProvider = (Microsoft.UI.Xaml.Application.Current as App)?.serviceProvider;

            TBDao = serviceProvider?.GetService<IThongBaoDao>();
            itemList = new ObservableCollection<ThongBao>(LoadData().Cast<ThongBao>());
            LoadDataLabelComponent();
            levelSelected = null;
            selectedDepartments = new ObservableCollection<LabelComponent>();
            selectedGroups = new ObservableCollection<LabelComponent>();
        }
        public int CreateItem(object item)
        {
            string content = (string)item;
            string label = levelSelected.SHORT_NAME + ":";
            for (int i = 0; i < selectedDepartments.Count; i++)
            {
                label += selectedDepartments[i].SHORT_NAME;

                if (i == selectedDepartments.Count - 1)
                {
                    label += ":";
                }
                else
                {
                    label += ",";
                }
            }

            for(int i = 0;  i < selectedGroups.Count; i++)
            {
                label += selectedGroups[i].SHORT_NAME;

                if (i == selectedGroups.Count - 1)
                {
                    continue;
                }
                else
                {
                    label += ",";
                }
            }

            if(TBDao.SendNotification(content, label))
                return 1;
            return 0; 
        }

        public int DeleteItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateWhenBack()
        {
            itemList = new ObservableCollection<ThongBao>(LoadData().Cast<ThongBao>());
        }

        public List<object> LoadData()
        {
            List<object> TBList = TBDao.Load(null);

            return TBList.ToList();
        }
        public void LoadDataLabelComponent()
        {
            levelList = new ObservableCollection<LabelComponent>(TBDao.GetAllLevels());
            departmentList = new ObservableCollection<LabelComponent>(TBDao.GetAllDepartments());
            groupList = new ObservableCollection<LabelComponent>(TBDao.GetAllGroups());
        }
        public int UpdateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedItem(object selectedItem)
        {
            throw new NotImplementedException();
        }
    }
}
