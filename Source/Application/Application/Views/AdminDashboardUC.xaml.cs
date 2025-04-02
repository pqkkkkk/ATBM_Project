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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Application.ViewModels;
using Application.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class AdminDashboardUC : UserControl
    {
        public RoleDataViewModel roleDataViewModel { get; set; }
        public UserDataViewModel userDataViewModel { get; set; }
        public MainViewModel mainViewModel { get; set; }
        public string? selectedTab { get; set; }
        public AdminDashboardUC()
        {
            userDataViewModel = new UserDataViewModel();
            roleDataViewModel = new RoleDataViewModel();
            mainViewModel = new MainViewModel();
            selectedTab = "Users";
            
            this.InitializeComponent();

            objectUC.SetDataSource(selectedTab);
        }

        private void OnTabViewChanged(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button != null)
            {
                selectedTab = button.Tag.ToString();

                mainViewModel.UpdateSelectedTabView(selectedTab);

                objectUC.SetDataSource(selectedTab);
            }
        }

        private void ViewPrivsDetailOfSelectedObject()
        {
            mainViewModel.UpdateCanBack(true);
            objectUC.Visibility = Visibility.Collapsed;
            objectDetailUC.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.UpdateCanBack(false);
            objectDetailUC.Visibility = Visibility.Collapsed;
            objectUC.Visibility = Visibility.Visible;
        }
    }
}
