using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace FilamentWinUI3.Helpers;

internal class BooleanAsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) 
    {
        if(value is bool bln && parameter is string str)
        {
            var yesNoArray = str.Split(',');
            if (yesNoArray.Length == 2)
            {
                return bln ? yesNoArray[0] : yesNoArray[1];
            }
        }
        return value;
        //throw new NotImplementedException();
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
