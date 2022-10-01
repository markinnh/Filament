using DataDefinitions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    public class HandleNullBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter is string)
                return false;
            else if (parameter is string str)
            {
                if (value?.GetType().GetProperty(str) is System.Reflection.PropertyInfo info)
                {
                    if (info.PropertyType == typeof(bool))
                    {
                        return (bool)(info.GetValue(value, null) ?? false);
                    }
                    else
                        throw new NotSupportedException("Currently this converter only examines boolean values to correct for null results");
                }
                else
                    throw new NotSupportedException($"{GetType().Name} does not have the {parameter} property");

            }

            else
                throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
