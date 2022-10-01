using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    public class MultiBoolCombiner : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                List<bool> bools = new List<bool>();
                foreach (var item in values)
                    if (item is bool boolean)
                        bools.Add(boolean);
                    else
                        return false;
                var result = true;
                foreach (var value in bools)
                    result &= value;
                return result;
            }
            else
                return false ;

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
