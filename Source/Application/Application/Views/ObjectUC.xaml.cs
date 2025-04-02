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
using Application.Model;
using Application.ViewModels;
using Application.Views.Components;
using Microsoft.VisualBasic.FileIO;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class ObjectUC : UserControl
    {
        public delegate void DetailClickedEventHandler();
        public event DetailClickedEventHandler? DetailClickedEvent;

        public static readonly DependencyProperty userDataViewModelProperty = DependencyProperty.Register(
            nameof(userDataViewModel),
            typeof(UserDataViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));

        public static readonly DependencyProperty roleDataViewModelProperty = DependencyProperty.Register(
            nameof(roleDataViewModel),
            typeof(RoleDataViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
        public static readonly DependencyProperty mainViewModelProperty = DependencyProperty.Register(
            nameof(mainViewModel),
            typeof(MainViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
        public UserDataViewModel userDataViewModel
        {
            get => (UserDataViewModel)GetValue(userDataViewModelProperty);
            set => SetValue(userDataViewModelProperty, value);
        }

        public RoleDataViewModel roleDataViewModel
        {
            get => (RoleDataViewModel)GetValue(roleDataViewModelProperty);
            set => SetValue(roleDataViewModelProperty, value);
        }
        public MainViewModel mainViewModel
        {
            get => (MainViewModel)GetValue(mainViewModelProperty);
            set => SetValue(mainViewModelProperty, value);
        }
        public ObjectUC()
        {
            this.InitializeComponent();

        }
        public void SetDataSource(string? objectType)
        {
            switch (objectType)
            {
                case "Users":
                    this.DataContext = userDataViewModel;
                    dataList.SetDataSource(objectType);
                    break;
                case "Roles":
                    this.DataContext = roleDataViewModel;
                    dataList.SetDataSource(objectType);
                    break;
            }
        }

        private void DetailClickHandler(object sender, RoutedEventArgs e)
        {
            DetailClickedEvent?.Invoke();
        }

        private async void AddClickHandler(object sender, RoutedEventArgs e)
        {
            switch(mainViewModel.selectedTabView)
            {
                case "Users":
                    await createUserDialog.ShowAsync();
                    break;
                case "Roles":
                    await createRoleDialog.ShowAsync();
                    break;
                default:
                    break;
            }
        }

        private void UpdateClickHandler(object sender, RoutedEventArgs e)
        {

        }

        private async void DeleteClickHandler(object sender, RoutedEventArgs e)
        {
            await deleteWarningDialog.ShowAsync();
        }
    }
}
