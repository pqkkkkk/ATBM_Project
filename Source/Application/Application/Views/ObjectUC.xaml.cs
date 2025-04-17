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

        public static readonly DependencyProperty userDataViewModelProperty = DependencyProperty.Register(
            nameof(userDataViewModel),
            typeof(UserDataViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));

        public static readonly DependencyProperty roleDataViewModelProperty = DependencyProperty.Register(
            nameof(roleDataViewModel),
            typeof(RoleDataViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
        public static readonly DependencyProperty mainViewModelProperty = DependencyProperty.Register(
            nameof(mainViewModel),
            typeof(MainViewModel),
            typeof(ObjectUC),
            new PropertyMetadata(null));
        public UserDataViewModel userDataViewModel
        {
            get => (UserDataViewModel)GetValue(userDataViewModelProperty);
            set => SetValue(userDataViewModelProperty, value);
        }

        public RoleDataViewModel roleDataViewModel
        {
            get => (RoleDataViewModel)GetValue(roleDataViewModelProperty);
            set => SetValue(roleDataViewModelProperty, value);
        }
        public MainViewModel mainViewModel
        {
            get => (MainViewModel)GetValue(mainViewModelProperty);
            set => SetValue(mainViewModelProperty, value);
        }
        public ObjectUC()
        {
            this.InitializeComponent();
            notificationDialog.DataContext = this;

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
            }
        }

        private void DetailClickHandler(object sender, RoutedEventArgs e)
        {
            DetailClickedEvent?.Invoke();
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
                var suggestions = roleDataViewModel.GetSuggestions(sender.Text);
                sender.ItemsSource = suggestions;
            }
        }

        private void searchBox_Click(object sender, RoutedEventArgs e)
        {
            roleDataViewModel.search(searchBox.Text.ToLower().Trim());
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

            int result = userDataViewModel.CreateItem(new User(username, password));
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

            int result = userDataViewModel.UpdateItem(new User(mainViewModel.selectedItem.name, newPassword));

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

