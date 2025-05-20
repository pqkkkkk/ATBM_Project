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
using Application.ViewModels.User;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class TestUC : UserControl
    {
        public ObservableCollection<Model.OracleObject> tableList { get; set; }
        public String selectedTabView { get; set; }
        public GVViewModel viewModel { get; set; }
        public TestUC()
        {
            var temp = (Microsoft.UI.Xaml.Application.Current as App)?.tableList;
            selectedTabView = "DANGKY";
            tableList = new ObservableCollection<Model.OracleObject>(temp);
            viewModel = new GVViewModel();
            this.InitializeComponent();
        }

        private void OnTabViewChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadPrivilegeOfRole();
        }

        private void btnTest2_Click(object sender, RoutedEventArgs e)
        {
            string str = "SELECT * FROM X_ADMIN.BOMON WHERE ID = 1";

            int spaceIdx1 = str.IndexOf("FROM") + 5;
            int spaceIdx2 = str.IndexOf(" ", spaceIdx1 + 1);
            if (spaceIdx2 == -1)
                spaceIdx2 = str.Length;
            string tableName = str.Substring(spaceIdx1, spaceIdx2 - spaceIdx1).Trim();
        }
    }
}
