using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    public class SplitAtUpperConverter:IValueConverter
    {
        const string splitPattern = @"(?<!^)(?=[A-Z])";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string str)
            {
                var words = Regex.Split(str, splitPattern);
                if (words.Length > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < words.Length; i++)
                    {
                        builder.Append(words[i]);
                        if (i + 1 < words.Length)
                            builder.Append(" ");
                    }
                    return builder.ToString();
                }
                else
                    return str;
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
