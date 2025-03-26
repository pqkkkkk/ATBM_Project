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
using Application.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public delegate void SignInClickedHandler(string username, string password);
        public event SignInClickedHandler signInClicked;

        public MainWindow()
        {
            this.InitializeComponent();
        }
        public void SignInSuccessHandler()
        {
            mainContent.Children.Clear();
            mainContent.Children.Add(new AdminDashboardUC());
        }
        public async void SignInFailedHandler()
        {
            ContentDialog signInFailedDialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Sign In Failed",
                Content = "Invalid username or password",
                CloseButtonText = "Ok"
            };
            await signInFailedDialog.ShowAsync();
        }
        private void SignInClickedHandlerInMainWindow(string username, string password)
        {
            signInClicked?.Invoke(username, password);
        }
    }
}
