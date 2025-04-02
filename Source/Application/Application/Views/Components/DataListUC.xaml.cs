using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Application.ViewModels;
using Application.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.Components
{
    public sealed partial class DataListUC : UserControl
    {
        public static readonly DependencyProperty userDataViewModelProperty = DependencyProperty.Register(
            nameof(userViewModel),
            typeof(UserDataViewModel),
            typeof(DataListUC),
            new PropertyMetadata(null));

        public static readonly DependencyProperty roleDataViewModelProperty = DependencyProperty.Register(
            nameof(roleViewModel),
            typeof(RoleDataViewModel),
            typeof(DataListUC),
            new PropertyMetadata(null));
        public static readonly DependencyProperty mainViewModelProperty = DependencyProperty.Register(
            nameof(mainViewModel),
            typeof(MainViewModel),
            typeof(DataListUC),
            new PropertyMetadata(null));
        public UserDataViewModel userViewModel
        {
            get => (UserDataViewModel)GetValue(userDataViewModelProperty);
            set => SetValue(userDataViewModelProperty, value);
        }

        public RoleDataViewModel roleViewModel
        {
            get => (RoleDataViewModel)GetValue(roleDataViewModelProperty);
            set => SetValue(roleDataViewModelProperty, value);
        }
        public MainViewModel mainViewModel
        {
            get => (MainViewModel)GetValue(mainViewModelProperty);
            set => SetValue(mainViewModelProperty, value);
        }
        public DataListUC()
        {
            this.InitializeComponent();
        }
        public void SetDataSource(string? objectType)
        {
            switch (objectType)
            {
                case "Users":
                    this.DataContext = userViewModel;
                    break;
                case "Roles":
                    this.DataContext = roleViewModel;
                    break;
                default:
                    break;
            }
        }
        private void SelectedChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataList.SelectedItem;
            if(selectedItem == null)
            {
                return;
            }

            CommonInfo newCommonInfo = new CommonInfo();

            switch (mainViewModel.selectedTabView)
            {
                case "Users":
                    User selectedUser = (User)selectedItem;
                    userViewModel.UpdateSelectedItem(selectedUser);

                    newCommonInfo.name = selectedUser.username;
                    newCommonInfo.objectType = "User";
                    mainViewModel.UpdateSelectedItem(newCommonInfo);

                    break;
                case "Roles":
                    Role selectedRole = (Role)selectedItem;
                    roleViewModel.UpdateSelectedItem(selectedRole);

                    newCommonInfo.name = selectedRole.name;
                    newCommonInfo.objectType = "Role";
                    mainViewModel.UpdateSelectedItem(newCommonInfo);
                    break;
                default:
                    break;
            }
        }
    }
}
