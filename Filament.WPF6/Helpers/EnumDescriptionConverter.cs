using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;

namespace Filament.WPF6.Helpers
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    // this should be a safe call since it is an enum
#pragma warning disable CS8604 // Possible null reference argument.
                    if (value.GetType().GetMember(value.ToString()).FirstOrDefault() is System.Reflection.MemberInfo member)
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return member.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
                    }
                }
            }
            return value;
#pragma warning restore CS8603 // Possible null reference return.

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
