using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Application.Helper
{
    class Helper
    {
        public Helper()
        {
        }
        public string GetTableNameFromTextOfView(string text)
        {
            int spaceIdx1 = text.IndexOf("FROM") + 5;
            int spaceIdx2 = text.IndexOf(" ", spaceIdx1 + 1);
            if (spaceIdx2 == -1)
                spaceIdx2 = text.Length;

            string tableName = text.Substring(spaceIdx1, spaceIdx2 - spaceIdx1).Trim();

            return tableName;
        }
        public static void bindingConverterWhileAutoGeneratingColumn(ResourceDictionary resources, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridTextColumn textColumn)
            {
                var binding = new Binding
                {
                    Path = new PropertyPath(e.PropertyName),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                };

                if (e.PropertyType == typeof(int) || e.PropertyType == typeof(int?))
                    binding.Converter = resources["IntToStringConverter"] as IValueConverter;

                else if (e.PropertyType == typeof(double) || e.PropertyType == typeof(double?))
                    binding.Converter = resources["DoubleToStringConverter"] as IValueConverter;

                else if (e.PropertyType == typeof(DateTime) || e.PropertyType == typeof(DateTime?))
                    binding.Converter = resources["DatetimeConverter"] as IValueConverter;

                textColumn.Binding = binding;
            }
        }
    }
}
