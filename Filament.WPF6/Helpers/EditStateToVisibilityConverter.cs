using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Filament.WPF6.Helpers
{
    public class EditStateToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PrintSettingEditState state)
            {
                if (parameter is string str && str.ToUpper() == "Definition")
                    return state == PrintSettingEditState.EditDefinition ? Visibility.Visible : Visibility.Collapsed;
                else
                    return state == PrintSettingEditState.EditSetting ? Visibility.Visible : Visibility.Collapsed;
            }
            else
                return value;

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EditStateToBooleanConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PrintSettingEditState state)
            {
                if (parameter is string str && str.ToUpper() == "Definition")
                    return state == PrintSettingEditState.EditDefinition;
                else
                    return state == PrintSettingEditState.EditSetting;
            }
            else
                return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
