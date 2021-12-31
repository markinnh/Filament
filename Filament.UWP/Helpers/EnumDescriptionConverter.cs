using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using Windows.UI.Xaml.Data;

namespace Filament.UWP.Helpers
{
    public class EnumDescriptionConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    if (value.GetType().GetMember(value.ToString()).FirstOrDefault() is MemberInfo memberInfo)
                        return memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
                }
            }

            return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
