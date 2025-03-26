using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Application.Converts
{
    public class TabViewBGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Style accentButtonStyle = (Style)(Microsoft.UI.Xaml.Application.Current as App).Resources["AccentButtonStyle"];
            Style defaultButtonStyle = (Style)(Microsoft.UI.Xaml.Application.Current as App).Resources["DefaultButtonStyle"];

            if (value is string selectedTabView && parameter is string actualTabView)
            {
                return selectedTabView == actualTabView ? accentButtonStyle : defaultButtonStyle;
            }

            return defaultButtonStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
