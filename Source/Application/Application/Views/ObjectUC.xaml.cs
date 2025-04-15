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

        private void UpdateClickHandler(object sender, RoutedEventArgs e)
        {
            DetailClickedEvent?.Invoke();
        }

        private async void DeleteClickHandler(object sender, RoutedEventArgs e)
        {
            if(roleDataViewModel.selectedRole != null)
            {
                deleteWarningDialog.ShowAsync();
            }
        }

        public string notificationTitle { get; set; } = string.Empty;
        public string notificationMessage { get; set; } = string.Empty;
        private async void createRoleDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool created = roleDataViewModel.CreateItem(roleTextBox.Text.Trim());
            if (created)
            {

                roleDataViewModel.itemList = new ObservableCollection<Role>(roleDataViewModel.LoadData().Cast<Role>());
                createRoleDialog.Hide();
                notificationDialog.Title = "Success";
                notificationTextBlock.Text = $"{roleTextBox.Text.Trim()} was created successfully";
                await notificationDialog.ShowAsync();
            }
            else
            {
                createRoleDialog.Hide();
                notificationDialog.Title = "Error";
                notificationTextBlock.Text = "There was something wrong in creating";
                await notificationDialog.ShowAsync();

            }
            roleTextBox.Text = string.Empty;
        }
        private async void deleteWarning_click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            switch (mainViewModel.selectedTabView)
            {
                case "Roles":
                    if (roleDataViewModel.DeleteItem(roleDataViewModel.selectedRole))
                    {
                        deleteWarningDialog.Hide();
                        notificationDialog.Title = "Success";
                        notificationTextBlock.Text = $"{roleDataViewModel.selectedRole.name} was deleted successfully";
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
                    break;
            };

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
    }
}
