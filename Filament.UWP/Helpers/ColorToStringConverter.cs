using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Filament.UWP.Helpers
{
    public class ColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            //else if(value is string str && DataDefinitions.PossibleARGB.TryParse(str,out DataDefinitions.PossibleARGB possibleARGB))
            //{

            //}
            if (value is string str && str.Contains('#'))
                return str;
            else
                return value;

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Color color)
            {
                return color.ToString();
            }
            else
                return value;
            //throw new NotImplementedException();
        }

    }
}
