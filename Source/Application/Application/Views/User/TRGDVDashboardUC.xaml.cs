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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.User
{
    public sealed partial class TRGDVDashboardUC : UserControl
    {
        public delegate void TabViewChangedEventHandler(string tabView);
        public event TabViewChangedEventHandler? TabViewChanged;
        public TRGDVViewModel viewModel { get; set; }
        public TRGDVDashboardUC()
        {
            viewModel = new TRGDVViewModel();
            this.InitializeComponent();
            TabViewChanged += dataContent.SetDataSource;
        }
        private async void OnAddClicked()
        {
            int addResult = viewModel.AddItem();

            if (addResult == 0)
            {
                var notification = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Failed",
                    Content = "You don't have permission for add to this table",
                    CloseButtonText = "OK"
                };
                await notification.ShowAsync();

                return;
            }
        }
        private void OnTabViewChanged(object sender, RoutedEventArgs e)
        {
            string selectedTab = (sender as Button).Tag.ToString();
            viewModel.UpdateSelectedTabView(selectedTab);
            TabViewChanged?.Invoke(selectedTab);
        }

        private async void OnDeleteClicked(object item)
        {
            int deleteResult = viewModel.DeleteItem();

            if (deleteResult == 0)
            {
                var notification = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Failed",
                    Content = "You don't have permission for delete this item",
                    CloseButtonText = "OK"
                };
                await notification.ShowAsync();
            }
        }

        private void CheckTheColumnOfRowIsEditable(object sender, Event.BeginningEditEvent e)
        {
            e.canEdit = viewModel.CheckTheColumnOfRowIsEditable(e.columnName);
        }
    }

}
