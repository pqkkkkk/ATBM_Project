using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess.MetaData;
using Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;

namespace Application.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public UserMetaData userMetaData { get; set; }
        private IMetaDataDao? metaDataDao;
        public string? selectedTabView { get; set; }
        public UserViewModel()
        {
            selectedTabView = "Users";
            userMetaData = new UserMetaData()
            {
                roles = new List<string>(),
                privileges = new List<UserPrivilege>(),
                username = "pqkiet854"
            };
        }
        public void UpdateSelectedTabView(string? tabView)
        {
            string? selectedTab = tabView;
            selectedTabView = selectedTab;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
