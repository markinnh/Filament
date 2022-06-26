using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    public class EnhVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (value is bool boolean)
                    return boolean ? Visibility.Visible : Visibility.Hidden;

                else if (value.ToString().Contains("NewItemPlaceholder"))
                    return Visibility.Hidden;
                else
                    return value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            else
                return Visibility.Collapsed;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
