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
        public DataViewModel<User> userDataViewModel { get; set; }
        public DataViewModel<Role> roleDataViewModel { get; set; }
        public UserViewModel userViewModel { get; set; }
        public string? selectedTab { get; set; }
        public AdminDashboardUC()
        {
            userDataViewModel = new DataViewModel<User>();
            roleDataViewModel = new DataViewModel<Role>();
            userViewModel = new UserViewModel();
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

                userViewModel.UpdateSelectedTabView(selectedTab);

                objectUC.SetDataSource(selectedTab);
            }
        }
    }
}
