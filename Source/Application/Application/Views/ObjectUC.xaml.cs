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
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Windows.Gaming.Input.ForceFeedback;
using Windows.Graphics.Printing.OptionDetails;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class ObjectUC : UserControl
    {
        public delegate void DetailClickedEventHandler();
        public event DetailClickedEventHandler? DetailClickedEvent;
        public string notificationTitle { get; set; } = string.Empty;
        public string notificationMessage { get; set; } = string.Empty;

        public static readonly DependencyProperty mainViewModelProperty = DependencyProperty.Register(
            nameof(mainViewModel),
            typeof(MainViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
        public UserDataViewModel userDataViewModel { get; set; }
        public  TableViewViewModel tableViewViewModel { get; set; }
        public PrivilegeDataViewModel privilegeDataViewModel { get; set; }
        public ProcedureFunctionViewModel procedureFunctionViewModel { get; set; }

        public RoleDataViewModel roleDataViewModel { get; set; }
        public MainViewModel mainViewModel
        {
            get => (MainViewModel)GetValue(mainViewModelProperty);
            set => SetValue(mainViewModelProperty, value);
        }
        public ObjectUC()
        {
            userDataViewModel = new UserDataViewModel();
            roleDataViewModel = new RoleDataViewModel();
            tableViewViewModel = new TableViewViewModel();
            privilegeDataViewModel = new PrivilegeDataViewModel();
            procedureFunctionViewModel = new ProcedureFunctionViewModel();
            this.InitializeComponent();
            notificationDialog.DataContext = this;
            this.Loaded += ObjectUC_Loaded;

        }

        private void ObjectUC_Loaded(object sender, RoutedEventArgs e)
        {
            SetDataSource(mainViewModel.selectedTabView);
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
                case "TablesAndViews":
                    this.DataContext = tableViewViewModel;
                    dataList.SetDataSource(objectType);
                    break;
                case "Procedures":
                    this.DataContext = procedureFunctionViewModel;
                    procedureFunctionViewModel.objectType = "Procedure";
                    procedureFunctionViewModel.itemList = new ObservableCollection<OracleObject>(procedureFunctionViewModel.LoadData().Cast<OracleObject>());
                    dataList.SetDataSource(objectType);
                    break;
                case "Functions":
                    this.DataContext = procedureFunctionViewModel;
                    procedureFunctionViewModel.objectType = "Function";
                    procedureFunctionViewModel.itemList = new ObservableCollection<OracleObject>(procedureFunctionViewModel.LoadData().Cast<OracleObject>());
                    dataList.SetDataSource(objectType);
                    break;
            }
        }

        private async void DetailClickHandler(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.selectedTabView == "Users" || mainViewModel.selectedTabView == "Roles")
            {
                DetailClickedEvent?.Invoke();
                return;
            }
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Information",
                Content = "No detail option for this tab!",
                CloseButtonText = "OK"
            };

            await contentDialog.ShowAsync();
            return;

        }

        private async void AddClickHandler(object sender, RoutedEventArgs e)
        {
            switch (mainViewModel.selectedTabView)
            {
                case "Users":
                    await createUserDialog.ShowAsync();
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
            if(mainViewModel.selectedItem == null)
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "No item selected",
                    CloseButtonText = "OK"
                };

                await contentDialog.ShowAsync();
                return;
            }
            else if(mainViewModel.selectedItem.objectType == "role")
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "You cannot update role",
                    CloseButtonText = "OK"
                };
                await contentDialog.ShowAsync();
                return;
            }

            switch (mainViewModel.selectedTabView)
            {
                case "Users":
                    await updateUserDialog.ShowAsync();
                    break;
                case "Roles":
                    break;
                default:
                    break;
            }
        }

        private async void DeleteClickHandler(object sender, RoutedEventArgs e)
        {
            if (mainViewModel.selectedItem == null)
            {
                ContentDialog noItemDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "No item selected",
                    CloseButtonText = "Ok"
                };
                await noItemDialog.ShowAsync();

                return;
            }
            else if (mainViewModel.selectedItem.objectType == "user")
            {
                deleteWarningDialog.Title = "Delete User";
                deleteWarningDialogTextBlock.Text = $"Are you sure you want to delete {mainViewModel.selectedItem.name}?";
                await deleteWarningDialog.ShowAsync();
            }
            else if (mainViewModel.selectedItem.objectType == "role")
            {
                deleteWarningDialog.Title = "Delete Role";
                deleteWarningDialogTextBlock.Text = $"Are you sure you want to delete {mainViewModel.selectedItem.name}?";
                await deleteWarningDialog.ShowAsync();
            }
        }
        private async void createRoleDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int created = roleDataViewModel.CreateItem(roleTextBox.Text.Trim());
            CreateRoleResult createRoleResult = (CreateRoleResult)created;

            if (createRoleResult != CreateRoleResult.Success)
            {
                createRoleResultTextBlock.Text = createRoleResult.ToString();
                args.Cancel = true;

                createRoleResultTextBlock.Visibility = Visibility.Visible;

                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    createRoleResultTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                createRoleResultTextBlock.Text = "Role created successfully";
                createRoleDialog.Hide();
                notificationDialog.Title = "Success";
                notificationTextBlock.Text = $"{roleTextBox.Text} was created successfully";
                await notificationDialog.ShowAsync();
            }

            roleTextBox.Text = string.Empty;
        }
        private async void deleteWarning_click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (mainViewModel.selectedItem.objectType == "role")
            {
                string deletedRole = roleDataViewModel.selectedRole.name;

                int result = roleDataViewModel.DeleteItem(roleDataViewModel.selectedRole);

                if (result == 1)
                {
                    deleteWarningDialog.Hide();
                    notificationDialog.Title = "Success";
                    notificationTextBlock.Text = $"{deletedRole} was deleted successfully";
                    await notificationDialog.ShowAsync();
                }
                else
                {
                    deleteWarningDialog.Hide();
                    notificationDialog.Title = "Error";
                    notificationTextBlock.Text = "There was something wrong in deleting";
                    await notificationDialog.ShowAsync();
                }
                roleDataViewModel.LoadData();
            }
            if (mainViewModel.selectedItem.objectType == "user")
            {
                string deletedUser = userDataViewModel.selectedUser.username;
                int result = userDataViewModel.DeleteItem(userDataViewModel.selectedUser);

                if (result == 1)
                {
                    deleteWarningDialog.Hide();
                    notificationDialog.Title = "Success";
                    notificationTextBlock.Text = $"{deletedUser} was deleted successfully";
                    await notificationDialog.ShowAsync();
                }
                else
                {
                    deleteWarningDialog.Hide();
                    notificationDialog.Title = "Error";
                    notificationTextBlock.Text = "There was something wrong in deleting";
                    await notificationDialog.ShowAsync();
                }
            }
            mainViewModel.UpdateSelectedItem(new CommonInfo { name = "", objectType = "" });
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                IEnumerable<string> suggestions = null;
                switch (mainViewModel.selectedTabView)
                {
                    case "Roles":
                        suggestions = roleDataViewModel.GetSuggestions(sender.Text);
                        break;
                    case "Users":
                        suggestions = userDataViewModel.GetSuggestions(sender.Text);
                        break;
                    case "TablesAndViews":
                        suggestions = tableViewViewModel.GetSuggestions(sender.Text);
                        break;
                    case "Procedures":
                    case "Functions":
                        suggestions = procedureFunctionViewModel.GetSuggestions(sender.Text);
                        break;
                }
                sender.ItemsSource = suggestions;
            }
        }

        private void searchBox_Click(object sender, RoutedEventArgs e)
        {
            switch (mainViewModel.selectedTabView)
            {
                case "Roles":
                    roleDataViewModel.search(searchBox.Text.ToLower().Trim());
                    break;
                case "Users":
                    userDataViewModel.search(searchBox.Text.ToLower().Trim());
                    break;
                case "TablesAndViews":
                    tableViewViewModel.search(searchBox.Text.ToLower().Trim());
                    break;
                case "Procedures":
                case "Functions":
                    procedureFunctionViewModel.search(searchBox.Text.ToLower().Trim());
                    break;
            }
        }

        private void SelectedItemChangeHandler(object selectedItem)
        {
            if (selectedItem is Model.User user)
            {
                mainViewModel.UpdateSelectedItem(new CommonInfo
                {
                    name = user.username,
                    objectType = "user",
                    isUser = true
                });
                userDataViewModel.UpdateSelectedItem(user);
            }
            else if (selectedItem is Model.Role role)
            {
                mainViewModel.UpdateSelectedItem(new CommonInfo
                {
                    name = role.name,
                    objectType = "role",
                    isUser = false
                });
                roleDataViewModel.UpdateSelectedItem(role);
            }
            else
            {
                mainViewModel.UpdateSelectedItem(new CommonInfo { name = "", objectType = "" });
            }
        }

        private async void CreateUserDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string username = usernameTextBoxWhenCreateUser.Text.Trim();
            string password = passwordTextBoxWhenCreateUser.Password.Trim();

            int result = userDataViewModel.CreateItem(new Model.User(username, password));
            CreateUserResult exception = (CreateUserResult)result;

            if (exception != CreateUserResult.Success)
            {
                args.Cancel = true;
                createUserResultTextBlockWhenCreateUser.Text = exception.ToString();

                createUserResultTextBlockWhenCreateUser.Visibility = Visibility.Visible;

                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    createUserResultTextBlockWhenCreateUser.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                createUserResultTextBlockWhenCreateUser.Text = "User created successfully";
                createUserDialog.Hide();
                notificationDialog.Title = "Success";
                notificationTextBlock.Text = $"{username} was created successfully";
                await notificationDialog.ShowAsync();
            }
        }

        private async void UpdateUserDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string newPassword = passwordTextBoxWhenUpdateUser.Password.Trim();

            int result = userDataViewModel.UpdateItem(new Model.User(mainViewModel.selectedItem.name, newPassword));

            if ((UpdateUserResult)result != UpdateUserResult.Success)
            {
                args.Cancel = true;

                updateUserResultWhenUpdateUserTextBlock.Text = ((UpdateUserResult)result).ToString();

                updateUserResultWhenUpdateUserTextBlock.Visibility = Visibility.Visible;
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(2000);
                    updateUserResultWhenUpdateUserTextBlock.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                updateUserResultWhenUpdateUserTextBlock.Text = "User updated successfully";
                updateUserDialog.Hide();
                notificationDialog.Title = "Success";
                notificationTextBlock.Text = $"{mainViewModel.selectedItem.name} was updated successfully";
                mainViewModel.UpdateSelectedItem(new CommonInfo { name = "", objectType = "" });
                await notificationDialog.ShowAsync();
            }
        }
    }
}

