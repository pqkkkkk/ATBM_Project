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
using Application.Model;
using CommunityToolkit.WinUI.UI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Application.Views.User
{
    public sealed partial class DataContentUC : UserControl
    {
        public delegate void UpdatedClickedHandler();
        public event UpdatedClickedHandler UpdateClicked;
        public delegate void DeletedClickedHandler(object item);
        public event DeletedClickedHandler DeleteClicked;
        public delegate void AddedClickedHandler();
        public event AddedClickedHandler AddClicked;

        public delegate void RowEditEnededHandler(object item);
        public event DeletedClickedHandler RowEditEnded;


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
        public static readonly DependencyProperty moMonListDependencyProperty =
            DependencyProperty.Register(nameof(moMonList),
                typeof(ObservableCollection<Model.MoMon>),
                typeof(DataContentUC),
                new PropertyMetadata(null));
        public ObservableCollection<Model.SinhVien> sinhVienList { get; set; }
        public ObservableCollection<Model.DangKy> dangKyList { get; set; }
        public ObservableCollection<Model.NhanVien> nhanVienList { get; set; }
        public ObservableCollection<Model.MoMon> moMonList { get; set; }
        public DataContentUC()
        {
            this.InitializeComponent();
        }
        public void SetDataSource(string tabView)
        {
            dataList.Columns.Clear();
            switch (tabView)
            {
                case "SinhVien":
                    dataList.ItemsSource = sinhVienList;
                    break;
                case "DangKy":
                    dataList.ItemsSource = dangKyList;
                    break;
                case "NhanVien":
                    dataList.ItemsSource = nhanVienList;
                    break;
                case "MoMon":
                    dataList.ItemsSource = moMonList;
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
            var selectedItem = dataList.SelectedItem;
            DeleteClicked?.Invoke(selectedItem);
        }
        private void DataGridRowEditEndedHandler(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndedEventArgs args)
        {
            var item = args.Row.DataContext;
            RowEditEnded?.Invoke(item);
        }
        private void OnAutoGenerateColumns(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "isInDB")
            {
                e.Cancel = true;
            }
            var dataGrid = (DataGrid)sender;
            var firstItem = dataGrid.ItemsSource?.Cast<object>()?.FirstOrDefault();
            if (firstItem == null)
                return;

            if (firstItem.GetType().Name == "MoMon")
            {
                var targetProperties = new HashSet<string> { "hk", "nam" };

                if (targetProperties.Contains(e.PropertyName))
                {
                    e.Cancel = true;

                    if (!dataGrid.Columns.Any(c => c.Header?.ToString() == e.PropertyName))
                    {
                        var binding = new Binding
                        {
                            Path = new PropertyPath(e.PropertyName),
                            Mode = BindingMode.TwoWay,
                            Converter = new NullableIntToStringConverter(),
                            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                        };

                        var column = new DataGridTextColumn
                        {
                            Header = e.PropertyName,
                            Binding = binding
                        };
                        dataGrid.Columns.Add(column);
                    }
                }
            }
        }
        public void UpdateSelectedItemOfDataListAfterAddNewItem()
        {
            dataList.SelectedItem = dataList.ItemsSource.Cast<object>().LastOrDefault();
        }
    }
}
