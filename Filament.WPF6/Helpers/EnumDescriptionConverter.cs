using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;

namespace Filament.WPF6.Helpers;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
#pragma warning disable CS8603 // Possible null reference return.
        if (value is Enum target &&
                    target.GetType().GetMember(target.ToString()).FirstOrDefault() is MemberInfo member &&
                    member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description)
        {
            return description.Description;
        }
        else
            return value;
#pragma warning restore CS8603 // Possible null reference return.

        //throw new NotImplementedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
