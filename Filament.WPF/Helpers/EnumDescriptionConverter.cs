using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF.Helpers
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().IsEnum)
            {
                // this should be a safe call since it is an enum
                if (value.GetType().GetMember(value.ToString()).FirstOrDefault() is System.Reflection.MemberInfo member)
                {
                    return member.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
                }
            }
            return value;

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
