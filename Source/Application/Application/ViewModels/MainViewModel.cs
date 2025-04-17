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
    public class MainViewModel : INotifyPropertyChanged
    {
        public string? selectedTabView { get; set; }
        public bool canBack { get; set; }
        public CommonInfo? selectedItem { get; set; }
        public MainViewModel()
        {
            canBack = false;
            selectedTabView = "Users";
            selectedItem = null;
        }
        public void UpdateSelectedTabView(string? tabView)
        {
            string? selectedTab = tabView;
            selectedTabView = selectedTab;
        }
        public void UpdateSelectedItem(CommonInfo? item)
        {
            selectedItem = item;
        }
        public void UpdateCanBack(bool canBack)
        {
            this.canBack = canBack;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
