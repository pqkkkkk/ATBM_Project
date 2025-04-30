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
using Application.DataAccess;
using Application.DataAccess.DangKy;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.User
{
    public sealed partial class DataContentUC : UserControl
    {
        public delegate void UpdatedClickedHandler();
        public event UpdatedClickedHandler UpdateClicked;
        public delegate void DeletedClickedHandler();
        public event DeletedClickedHandler DeleteClicked;
        public delegate void AddedClickedHandler();
        public event AddedClickedHandler AddClicked;


        public static readonly DependencyProperty sinhVienListDependencyProperty =
            DependencyProperty.Register(nameof(sinhVienList),
                typeof(ObservableCollection<Model.SinhVien>),
                typeof(DataContentUC),
                new PropertyMetadata(null));
        public static readonly DependencyProperty dangKyListDependencyProperty = 
            DependencyProperty.Register(nameof(dangKyList),
                typeof(ObservableCollection<Model.DangKy>),
                typeof(DataContentUC),
                new PropertyMetadata(null));
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public DataContentUC()
        {
            this.InitializeComponent();
        }
        public void SetDataSource(string tabView)
        {
            switch (tabView)
            {
                case "SinhVien":
                    dataList.ItemsSource = sinhVienList;
                    break;
                case "DangKy":
                    dataList.ItemsSource = dangKyList;
                    break;
                default:
                    break;
            }
        }
        private void DetailClickHandler(object sender, RoutedEventArgs e)
        {

        }

        private void AddClickHandler(object sender, RoutedEventArgs e)
        {
            AddClicked?.Invoke();
        }

        private void UpdateClickHandler(object sender, RoutedEventArgs e)
        {
            UpdateClicked?.Invoke();
        }

        private void DeleteClickHandler(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke();
        }

    }
}
