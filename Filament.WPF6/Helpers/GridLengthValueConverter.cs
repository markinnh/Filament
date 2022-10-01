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
    public class GridLengthValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLengthConverter glc = new GridLengthConverter();
            if (value != null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                if (value is string str)
                    return glc.ConvertFromString(str);
                else if (value is double dbl)
                    return glc.ConvertFrom(dbl);
                else
                    throw new NotSupportedException($"Unable to convert {value.GetType().Name}");
#pragma warning restore CS8603 // Possible null reference return.
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLengthConverter glc = new GridLengthConverter();
            if(value != null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return glc.ConvertToString(value);
#pragma warning restore CS8603 // Possible null reference return.
            }
            throw new NotImplementedException();
        }
    }
}
