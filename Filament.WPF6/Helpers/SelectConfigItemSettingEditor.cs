using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Filament.WPF6.Helpers
{
    public class SelectConfigItemSettingEditor : DataTemplateSelector
    {
        public DataTemplate? IntegerDataTemplate { get; set; }
        public DataTemplate? BooleanDataTemplate { get; set; }
        public DataTemplate? FloatDataTemplate { get; set; }
        public DataTemplate? YesNoDataTemplate { get; set; }
        public DataTemplate? OnOffDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConfigItem configItem)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                var template = configItem.PrintSettingDefn.SettingValueType switch
                {
                    SupportedSettingValueType.Integer => IntegerDataTemplate,
                    SupportedSettingValueType.Boolean => BooleanDataTemplate,
                    SupportedSettingValueType.Float => FloatDataTemplate,
                    SupportedSettingValueType.YesNo => YesNoDataTemplate,
                    SupportedSettingValueType.OnOff => OnOffDataTemplate,
                    _ => throw new NotImplementedException()
                };
                return template ?? base.SelectTemplate(item, container);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                throw new NotImplementedException();
            }
            else if (item is PrintSettingDefn printSettingDefn)
            {
                throw new NotImplementedException();
            }
            else
                return base.SelectTemplate(item, container);
        }
        
    }
}
