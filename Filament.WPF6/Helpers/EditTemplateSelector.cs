using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Diagnostics.Debug;

namespace Filament.WPF6.Helpers
{
    public class EditTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? EditFilamentTemplate { get; set; }
        public DataTemplate? EditVendorTemplate { get; set; }
        public DataTemplate? EditSpoolDefnTemplate { get; set; }
        public DataTemplate? EditInventorySpoolTemplate { get; set; }
        public DataTemplate? NoEditorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is null)
                return base.SelectTemplate(item, container);
            else if (item is FilamentDefn)
#pragma warning disable CS8603 // Possible null reference return.
                return EditFilamentTemplate;
            else if (item is VendorDefn)
                return EditVendorTemplate;
            else if (item is SpoolDefn)
                return EditSpoolDefnTemplate;
            else if (item is InventorySpool)
                return EditInventorySpoolTemplate;
            else {
                WriteLine($"Need to develop a template for {item.GetType().Name}");
                return NoEditorTemplate;
#pragma warning restore CS8603 // Possible null reference return.
            }

        }
    }
}
