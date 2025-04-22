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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class AdminDashboardUC : UserControl
    {
        public MainViewModel mainViewModel { get; set; }
        public string? selectedTab { get; set; }
        public AdminDashboardUC()
        {
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
                mainViewModel.UpdateSelectedItem(null);
                objectUC.SetDataSource(selectedTab);
            }
        }

        private async void ViewPrivsDetailOfSelectedObject()
        {
            if(mainViewModel.selectedItem == null)
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "Please select a user or role in the list to view details.",
                    CloseButtonText = "OK"
                };
                await contentDialog.ShowAsync();
                return;
            }
            mainViewModel.UpdateCanBack(true);
            objectUC.Visibility = Visibility.Collapsed;
            objectDetailUC.Visibility = Visibility.Visible;
            objectDetailUC.UpdateAllData();
            //objectDetailUC.SetDataSourceForDataList();
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.UpdateCanBack(false);

            objectDetailUC.UpdateDataWhenBack();
            objectDetailUC.Visibility = Visibility.Collapsed;

            objectUC.Visibility = Visibility.Visible;
        }
    }
}
