using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions;
using Microsoft.UI.Xaml.Data;

namespace FilamentWinUI3.Helpers;
internal class WeightWithUnitsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => UnitConversion.FormatWeightWithUnits(value,parameter);
    public object ConvertBack(object value, Type targetType, object parameter, string language) => UnitConversion.ConvertWithUnitsOfWeight(value,parameter);
}
