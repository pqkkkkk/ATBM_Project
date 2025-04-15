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
                    ContentDialogResult result = await createUserDialog.ShowAsync();
                    if(result == ContentDialogResult.Primary)
                    {
                        string username = UsernameTextBox.Text;
                        string password = PasswordTextBox.Password;
                        var user = new User(username, password);
                        userDataViewModel.CreateItem(user);
                    }
                    break;
                case "Roles":
                    await createRoleDialog.ShowAsync();
                    break;
                default:
                    break;
            }
        }

        private async void UpdateClickHandler(object sender, RoutedEventArgs e)
        {
            switch (mainViewModel.selectedTabView)
            {
                case "Users":
                    var selectedUser = userDataViewModel.selectedUser;
                    if (selectedUser != null && !string.IsNullOrEmpty(selectedUser.username))
                    {
                        ContentDialogResult result = await updateUserDialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            string newpassword = UpdatePasswordTextBox.Password;
                            var user = new User(selectedUser.username, newpassword);
                            userDataViewModel.UpdateItem(user);
                        }
                    }
                    break;
                case "Roles":
                    break;
                default:
                    break;
            }
        }

        private async void DeleteClickHandler(object sender, RoutedEventArgs e)
        {
            switch (mainViewModel.selectedTabView)
            {
                case "Users":
                    var selectedUser = userDataViewModel.selectedUser;
                    if (selectedUser != null)
                    {
                        ContentDialogResult result = await deleteWarningDialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            userDataViewModel.DeleteItem(selectedUser);
                        }
                    }
                    break;
                case "Roles":
                    break;
                default:
                    break;
            }
        }
    }
}
