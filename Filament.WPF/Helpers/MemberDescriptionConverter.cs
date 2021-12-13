using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;
using System.ComponentModel;

namespace Filament.WPF.Helpers
{
    public class MemberDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType is MemberInfo info) {
                if (info.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
                    return descriptionAttribute.Description;
                else
                    return targetType.Name;
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
