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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class ObjectDetailUC : UserControl
    {
        public static readonly DependencyProperty mainViewModelProperty = DependencyProperty.Register(
            nameof(mainViewModel),
            typeof(MainViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
 
        public MainViewModel mainViewModel
        {
            get => (MainViewModel)GetValue(mainViewModelProperty);
            set => SetValue(mainViewModelProperty, value);
        }
        public PrivilegeViewModel privilegeViewModel { get; set; }
        public ObjectDetailUC()
        {
            privilegeViewModel = new PrivilegeViewModel();

            this.InitializeComponent();
        }

        private async void GrantRole_Click(object sender, RoutedEventArgs e)
        {
            await grantRoleDialog.ShowAsync();
        }

        private async void RevokeRole_Click(object sender, RoutedEventArgs e)
        {
             await revokeRoleDialog.ShowAsync();
        }

        private async void GrantPrivs_Click(object sender, RoutedEventArgs e)
        {
            switch (privilegeViewModel.selectObjectType)
            {
                case "Table":
                    await grantPrivOnTableDialog.ShowAsync();
                    break;
                case "View":
                    await grantPrivOnViewDialog.ShowAsync();
                    break;
                case "Procedure":
                    await grantPrivOnProcedureDialog.ShowAsync();
                    break;
                case "Function":
                    // Implement revoke cell privilege logic
                    break;
                case "System priv":
                    // Implement revoke system privilege logic
                    break;
                default:
                    ContentDialog noObjectTypeSelectedDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "No object type selected",
                        Content = "Please select an object type on left to grant privileges on.",
                        CloseButtonText = "Ok"
                    };
                    await noObjectTypeSelectedDialog.ShowAsync();

                    break;
            }
        }

        private void RevokePrivs_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ChangeSelectedObjectType(object sender, SelectionChangedEventArgs e)
        {
            if (selectObjectTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedObjectType = selectedItem.Content?.ToString() ?? "";

                privilegeViewModel.UpdateSelectedObjectType(selectedObjectType);
            }
        }
    }
}
