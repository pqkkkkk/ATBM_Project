using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using Application.Model;
using Application.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input.ForceFeedback;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class ThongBaoUC : UserControl
    {        public NotificationDataViewModel notificationDataViewModel { get; set; }
        public ThongBaoUC()
        {
            this.InitializeComponent();
            notificationDataViewModel = new NotificationDataViewModel();
        }

        private void DepartmentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is LabelComponent addedItem && !notificationDataViewModel.selectedDepartments.Contains(addedItem))
                {
                    notificationDataViewModel.selectedDepartments.Add(addedItem);
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is LabelComponent removedItem && notificationDataViewModel.selectedDepartments.Contains(removedItem))
                {
                    notificationDataViewModel.selectedDepartments.Remove(removedItem);
                }
            }
        }

        private void LocationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            foreach (var item in e.AddedItems)
            {
                if (item is LabelComponent addedItem && !notificationDataViewModel.selectedGroups.Contains(addedItem))
                {
                    notificationDataViewModel.selectedGroups.Add(addedItem);
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is LabelComponent removedItem && notificationDataViewModel.selectedGroups.Contains(removedItem))
                {
                    notificationDataViewModel.selectedGroups.Remove(removedItem);
                }
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (notificationDataViewModel.levelSelected == null)
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Alert",
                    Content = "Please choose which one you want to send",
                    CloseButtonText = "OK"
                };
                contentDialog.ShowAsync();
            }
            else
            {
                int result = notificationDataViewModel.CreateItem(NotificationContentTextBox.Text.Trim());
                if (result == 1)
                {
                    ContentDialog contentDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Success",
                        Content = "Notification sent successfully!",
                        CloseButtonText = "OK"
                    };
                    contentDialog.ShowAsync();
                    NotificationContentTextBox.Text = string.Empty;
                    notificationDataViewModel.levelSelected = null;
                    notificationDataViewModel.selectedDepartments.Clear();
                    notificationDataViewModel.selectedGroups.Clear();
                }
                else
                {
                    ContentDialog contentDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Error",
                        Content = "Failed to send notification. Please try again.",
                        CloseButtonText = "OK"
                    };
                    contentDialog.ShowAsync();
                }

            }
        }
    }
}
