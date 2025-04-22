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
using Application.Views.Components;
using System.Runtime.CompilerServices;
using System.ComponentModel;

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
        public PrivilegeDataViewModel privilegeViewModel { get; set; }
        public ObjectDetailUC()
        {
            privilegeViewModel = new PrivilegeDataViewModel();

            this.InitializeComponent();
            this.Loaded += ObjectDetailUC_Loaded;
        }
        private void ObjectDetailUC_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainViewModel != null)
            {
                privilegeViewModel = new PrivilegeDataViewModel(mainViewModel.selectedItem);
            }
        }
        public void UpdateAllData()
        {
            privilegeViewModel.UpdatedAllData(mainViewModel.selectedItem);
        }
        //public void SetDataSourceForDataList()
        //{
        //    dataList.SetDataSource("Privileges");
        //}
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
            switch (privilegeViewModel.selectedObjectType)
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
                case "System privilege":
                    await grantSystemPrivDialog.ShowAsync();
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

        private async void RevokePrivs_Click(object sender, RoutedEventArgs e)
        {
            await revokePrivilgeDialog.ShowAsync();
        }

        private void ChangeSelectedObjectType(object sender, SelectionChangedEventArgs e)
        {
            if (selectObjectTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedObjectType = selectedItem.Content?.ToString() ?? "";

                privilegeViewModel.UpdateSelectedObjectType(selectedObjectType);
            }
        }

        private void OnChangeSelectedTableOrViewWhenGrantPriv(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is Model.OracleObject selectedObject)
                {
                    privilegeViewModel.UpdateSelectedObjectWhenSelectionChange(selectedObject);
                }
            }
        }

        private void OnChangeSelectedRole(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is Model.Role selectedObject)
                {
                    privilegeViewModel.UpdateSelectedRole(selectedObject);
                }
            }
        }

        private async void OnGrantRoleCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string roleName = RoleComboBoxInGrantRole.SelectedItem is Model.Role selectedRole ? selectedRole.name : "";

            if (string.IsNullOrEmpty(roleName))
            {
                args.Cancel = true;

                errorWhenGrantRoleTextBlock.Text = "Please select a role to grant.";
                errorWhenGrantRoleTextBlock.Visibility = Visibility.Visible;

                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantRoleTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                bool isWithGrantOption = withGrantOptionCheckboxInGrantRole.IsChecked == true;
                privilegeViewModel.GrantRole(roleName, isWithGrantOption);
            }
        }

        private void OnRevokeRoleCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int result = privilegeViewModel.RevokeRole();
            if (result == 1)
                return;
            else if (result == 0)
            {
                args.Cancel = true;
                errorWhenRevokeRoleTextBlock.Text = "Please select a role to revoke.";
                errorWhenRevokeRoleTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenRevokeRoleTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenRevokeRoleTextBlock.Text = "Error when revoking role.";
                errorWhenRevokeRoleTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenRevokeRoleTextBlock.Visibility = Visibility.Collapsed;
                });
            }
        }

        private void OnGrantPrivOnProcedureCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string procedureName = ProcedureComboBoxInGrantPrivOnProcedure.SelectedItem is Model.OracleObject selectedObject ? selectedObject.objectName : "";
            bool isWithGrantOption = withGrantOptionCheckboxWhenGrantPrivOnProcedure.IsChecked == true;

            int result = privilegeViewModel.GrantPrivilegeOnProcedure(procedureName, isWithGrantOption);

            if (result == 0)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnProcedureTextBlock.Text = "Please select a procedure to grant privilege.";
                errorWhenGrantPrivOnProcedureTextBlock.Visibility = Visibility.Visible;

                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnProcedureTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnProcedureTextBlock.Text = "Error when granting privilege.";
                errorWhenGrantPrivOnProcedureTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnProcedureTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                privilegeViewModel.LoadData();
            }
        }

        private void OnChangeSelectedActionOnTableOrView(object sender, SelectionChangedEventArgs e)
        {
            var selectedAction = (sender as ComboBox)?.SelectedItem as ComboBoxItem;
            if (selectedAction != null)
            {
                string selectedActionText = selectedAction.Content?.ToString() ?? "";
                privilegeViewModel.UpdateSelectedActionOnTableOrView(selectedActionText);
            }
        }

        private void OnCloseGrantPrivOnTableOrViewDialog(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            privilegeViewModel.UpdateWhenCloseGrantPrivilegeOnTableOrViewDialog();
        }

        private void OnGrantPrivOnTableCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string action = actionListOnTableCombobox.SelectedItem is ComboBoxItem selectedAction ? selectedAction.Content?.ToString() : "";
            string tableName = TableListComboBox.SelectedItem is Model.OracleObject selectedObject ? selectedObject.objectName : "";
            bool isWithGrantOption = isWithGrantOptionWhenGrantPrivOnTable.IsChecked == true;

            int result = privilegeViewModel.GrantPrivilegeOnTableOrView(action, tableName, isWithGrantOption);

            if (result == 0)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnTableTextBlock.Text = "No selected table or action or no selected column.";
                errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnTableTextBlock.Text = "Error when granting privilege.";
                errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                privilegeViewModel.UpdateWhenCloseGrantPrivilegeOnTableOrViewDialog();
            }
        }

        private void OnGrantPrivOnViewCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string action = actionListOnViewCombobox.SelectedItem is ComboBoxItem selectedAction ? selectedAction.Content?.ToString() : "";
            string tableName = viewListComboBox.SelectedItem is Model.OracleObject selectedObject ? selectedObject.objectName : "";
            bool isWithGrantOption = isWithGrantOptionWhenGrantPrivOnTable.IsChecked == true;

            int result = privilegeViewModel.GrantPrivilegeOnTableOrView(action, tableName, isWithGrantOption);

            if (result == 0)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnTableTextBlock.Text = "No selected table or action or no selected column.";
                errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnTableTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenGrantPrivOnViewTextBlock.Text = "Error when granting privilege.";
                errorWhenGrantPrivOnViewTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantPrivOnViewTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                privilegeViewModel.UpdateWhenCloseGrantPrivilegeOnTableOrViewDialog();
            }
        }

        private void SelectedItemChangedHandler(object selectedItem)
        {
            if (selectedItem is Model.Privilege selectedPrivilege)
                privilegeViewModel.UpdateSelectedItem(selectedPrivilege);
        }

        private void OnRevokePrivilelgeCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int result = privilegeViewModel.DeleteItem(null);

            if (result == 0)
            {
                args.Cancel = true;
                errorWhenRevokePrivilegeTextBlock.Text = "Please select a privilege to revoke.";
                errorWhenRevokePrivilegeTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenRevokePrivilegeTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenRevokePrivilegeTextBlock.Text = "Error when revoking privilege.";
                errorWhenRevokePrivilegeTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenRevokePrivilegeTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                privilegeViewModel.LoadData();
            }

        }

        private void SelectedItemChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataList.SelectedItem;

            if (selectedItem is Model.Privilege selectedPrivilege)
                privilegeViewModel.UpdateSelectedItem(selectedPrivilege);
        }
        public void UpdateDataWhenBack()
        {
            privilegeViewModel.UpdateSelectedObjectType("");
            selectObjectTypeComboBox.SelectedItem = null;
        }

        private void OnGrantSystemPrivilege(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string privilege = SystemPrivComboBoxInGranSystemtPriv.SelectedItem is Model.Privilege selectedObject ? selectedObject.privilege : "";
            bool isWithAdminOption = withAdminOptionCheckboxWhenGrantSystemPriv.IsChecked == true;

            int result = privilegeViewModel.GrantSystemPrivilege(privilege, isWithAdminOption);

            if (result == 0)
            {
                args.Cancel = true;
                errorWhenGrantSystemPrivTextBlock.Text = "Please select a system privilege to grant.";
                errorWhenGrantSystemPrivTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantSystemPrivTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else if (result == -1)
            {
                args.Cancel = true;
                errorWhenGrantSystemPrivTextBlock.Text = "Error when granting system privilege.";
                errorWhenGrantSystemPrivTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    errorWhenGrantSystemPrivTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {

            }
        }
    }
}
