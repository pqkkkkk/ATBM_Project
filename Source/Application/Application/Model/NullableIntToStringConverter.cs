using System;
using Microsoft.UI.Xaml.Data;

namespace Application.Model
{
    public class NullableIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string input = value as string;

            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return null; // Nếu nhập sai kiểu, trả về null
        }
    }
}
