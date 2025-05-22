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
using Application.ViewModels.User;
using static Application.Views.User.SVDashboardUC;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.User
{
    public sealed partial class NVPKTDashboardUC : UserControl
    {
        public NVPKTViewModel nvPKTViewModel { get; set; }
        public event TabViewChangedEventHandler? TabViewChanged;
        public NVPKTDashboardUC()
        {
            nvPKTViewModel = new NVPKTViewModel();
            this.InitializeComponent();
            TabViewChanged += dataContent.SetDataSource;
            dataContent.BeginningEdit += CheckCanEditSelectedColumn;

        }

        private void OnTabViewChanged(object sender, RoutedEventArgs e)
        {
            string selectedTab = (sender as Button).Tag.ToString();
            nvPKTViewModel.UpdateSelectedTabView(selectedTab);
            TabViewChanged?.Invoke(selectedTab);
        }

        private async void SaveItem(object item)
        {
            int result = nvPKTViewModel.SaveItem(item);

            if (result == 1)
            {
                var notification = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Success",
                    Content = "Save successfully",
                    CloseButtonText = "OK",
                };

                await notification.ShowAsync();
            }
            else
            {
                var notification = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Failed",
                    Content = "Save failed",
                    CloseButtonText = "OK",

                };
                await notification.ShowAsync();
            }
        }
        private async void OnDeleteClicked(object item)
        {
            ContentDialog error = new ContentDialog
            {
                Title = "Notification",
                Content = "You don't have permission to delete item",
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot

            };
            await error.ShowAsync();

        }

        private async void OnAddClicked()
        {
            ContentDialog error = new ContentDialog
            {
                Title = "Notification",
                Content = "You don't have permission to add new item",
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot

            };
            await error.ShowAsync();


        }

        private void OnUpdateClicked()
        {

        }
        private void CheckCanEditSelectedColumn(object sender, Event.BeginningEditEvent e)
        {
            e.canEdit = nvPKTViewModel.CheckTheColumnOfRowIsEditable(e.columnName.ToUpper());
        }
    }
}

