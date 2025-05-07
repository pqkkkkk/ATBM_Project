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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.User
{
    public sealed partial class SVDashboardUC : UserControl
    {
        public delegate void TabViewChangedEventHandler(string tabView);
        public event TabViewChangedEventHandler? TabViewChanged;
        public SVViewModel viewModel { get; set; }
        public SVDashboardUC()
        {
            viewModel = new SVViewModel();
            this.InitializeComponent();
            TabViewChanged += dataContent.SetDataSource;
        }

        private void OnTabViewChanged(object sender, RoutedEventArgs e)
        {
            string selectedTab = (sender as Button).Tag.ToString();
            viewModel.UpdateSelectedTabView(selectedTab);
            TabViewChanged?.Invoke(selectedTab);
        }

        private void OnDeleteClicked(object item)
        {
            viewModel.DeleteItem();
        }

        private void OnAddClicked()
        {
            viewModel.AddItem();
        }

        private void OnUpdateClicked()
        {
            viewModel.UpdateItem();
        }
    }
}
