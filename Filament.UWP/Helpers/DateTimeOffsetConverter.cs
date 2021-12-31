using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Filament.UWP.Helpers
{
    /// <summary>
    /// Converts between DateTime and DateTimeOffset
    /// </summary>
    public class DateTimeOffsetConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
                return new DateTimeOffset(date);
            else
                return value;

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dtOffset)
                return dtOffset.DateTime;
            else
                return value;
            throw new NotImplementedException();
        }
    }
}
