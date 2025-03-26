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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views
{
    public sealed partial class ObjectUC : UserControl
    {
        public static readonly DependencyProperty userDataViewModelProperty = DependencyProperty.Register(
            nameof(userDataViewModel),
            typeof(DataViewModel<User>),
            typeof(DataListUC),
            new PropertyMetadata(null));

        public static readonly DependencyProperty roleDataViewModelProperty = DependencyProperty.Register(
            nameof(roleDataViewModel),
            typeof(DataViewModel<Role>),
            typeof(DataListUC),
            new PropertyMetadata(null));

        public DataViewModel<User> userDataViewModel
        {
            get => (DataViewModel<User>)GetValue(userDataViewModelProperty);
            set => SetValue(userDataViewModelProperty, value);
        }

        public DataViewModel<Role> roleDataViewModel
        {
            get => (DataViewModel<Role>)GetValue(roleDataViewModelProperty);
            set => SetValue(roleDataViewModelProperty, value);
        }

        public ObjectUC()
        {
            this.InitializeComponent();

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
    }
}
