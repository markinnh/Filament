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
            if (value != null && parameter is string str)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if(str.Contains(',') && str.Split(',').Length == 2 && value is bool bln)
                {
                    var args = str.Split(',');
                    var trueFlag = Enum.Parse<Visibility>(args[0]);
                    var falseFlag = Enum.Parse<Visibility>(args[1]);
                    return bln ? trueFlag : falseFlag;
                }
                else if (value is bool boolean && str == "NOT")
                    return boolean ? Visibility.Collapsed : Visibility.Visible;
                else if (value is bool boolean1 )
                    return boolean1 ? Visibility.Visible : Visibility.Collapsed;
                else if (value is bool boolean2 && str == "NORMAL")
                    return boolean2 ? Visibility.Visible : Visibility.Hidden;
                else if (value ==DependencyProperty.UnsetValue)
                    return Visibility.Hidden;
                else
                    return value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            else if (value != null && parameter == null)
            {
                if (value is bool boolean)
                    return boolean ? Visibility.Visible : Visibility.Collapsed;
                else if (value == DependencyProperty.UnsetValue)
                    return Visibility.Hidden;
                else
                    return Visibility.Collapsed;
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
