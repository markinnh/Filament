using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using DataDefinitions;
namespace Filament.UWP.Helpers
{
    public class WeightWithUnitsConverter : IValueConverter
    {
        static readonly string[] shortHandNames = { "g", "kg", "oz", "lb" };
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SupportedWeight expectedUnits = SupportedWeight.Kilogram;
            if (parameter is SupportedWeight supportedWeight)
            {
                expectedUnits = supportedWeight;
            }
            else if (parameter is string str)
            {
                if (Enum.TryParse<SupportedWeight>(str, out SupportedWeight supported))
                {
                    expectedUnits = supported;
                }
            }

            if (value is double weight)
            {
                if (!double.IsNaN(weight))
                    return $"{weight} {shortHandNames[(int)expectedUnits]}";
                else
                    return $"{weight}";
            }
            return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            ConvertToWeight expected = ConvertToWeight.Kilograms;
            if(parameter is ConvertToWeight supportedWeight)
                expected = supportedWeight;
            else if (parameter is string str)
                if(Enum.TryParse<ConvertToWeight>(str,out ConvertToWeight supported))
                    expected = supported;

            if(value is string content)
                if(ValueWithUnits.TryParse(content, out ValueWithUnits valueWithUnits))
                    return FilamentMath.ConvertWeight(valueWithUnits.Value,FilamentMath.SupportedWeightAlias(valueWithUnits.Units),expected);

            return double.NaN;
            //throw new NotImplementedException();
        }
    }
}
