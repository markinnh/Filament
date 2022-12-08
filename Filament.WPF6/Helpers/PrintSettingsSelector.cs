using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Filament.WPF6.Helpers
{
    public class PrintSettingsSelector : DataTemplateSelector
    {
        public DataTemplate? SupportedFilamentsTemplate { get; set; }
        public DataTemplate? PrintSettingsEditorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && SupportedFilamentsTemplate != null && PrintSettingsEditorTemplate != null)
            {
                if (item is VendorDefn)
                    return SupportedFilamentsTemplate;
                else if (item is VendorPrintSettingsConfig)
                    return PrintSettingsEditorTemplate;
                else
                    return base.SelectTemplate(item, container);
            }
            else
                return base.SelectTemplate(item, container);
        }
    }
}
