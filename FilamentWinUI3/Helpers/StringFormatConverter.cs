using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace FilamentWinUI3.Helpers;
public class StringFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string str && value != null)
        {
            try
            {
                return string.Format(str, value);
            }
            catch(Exception ex) {
                Debug.WriteLine($"Exception during format.  {ex.Message}");
                Debug.Assert(false);
            }
            return value;
        }
        else
            return value;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
