using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using DataDefinitions;
namespace Filament.UWP.Helpers
{

    public class LengthWithUnitsConverter : IValueConverter
    {
        static readonly string[] shortHandUnits = new[] { "mm", "cm", "m","in" };
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string units = string.Empty;
            if (parameter is string str)
            {
                if (Enum.TryParse<SupportedLength>(str, out SupportedLength parsedUnits))
                    units = shortHandUnits[(int)parsedUnits];
            }
            if (value is double d)
            {
                if (double.IsNaN(d))
                    return d;
                else
                    return $"{d:#.####} {units}";
            }
            else
                return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool unitsSet = false;
            ConvertToLength expectedUnits;
            //double conversionFactor = 1;
            if (parameter is string expected)
            {
                if (Enum.TryParse<ConvertToLength>(expected, out expectedUnits))
                {
                    unitsSet = true;
                }

            }
            else if (parameter is ConvertToLength actual)
            {
                expectedUnits = actual;
                unitsSet = true;
            }
            else
            {
                expectedUnits = ConvertToLength.Millimeter;
                unitsSet=true;
            }
            if (value is string str && unitsSet)
            {
                // TODO: implement a conversion factor for the units
                if (ValueWithUnits.TryParse(str, out ValueWithUnits valueWithUnits))
                {
                    return FilamentMath.ConvertLength(valueWithUnits.Value,FilamentMath.SupportedLengthAlias(valueWithUnits.Units),expectedUnits);
                    
                }
                else if (CompoundFractionWithUnits.TryParse(str, out CompoundFractionWithUnits compoundFractionWithUnits))
                {
                    return FilamentMath.ConvertLength(compoundFractionWithUnits.Value,FilamentMath.SupportedLengthAlias(compoundFractionWithUnits.Units),expectedUnits);
                }
            }
            return value;
            //throw new NotImplementedException();
        }
    }
}
