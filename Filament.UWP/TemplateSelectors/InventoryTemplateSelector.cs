using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataDefinitions.Models;

namespace Filament.UWP.TemplateSelectors
{
    public class InventoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VendorDefnTemplate { get; set; }
        public DataTemplate SpoolDefnTemplate { get; set; }
        public DataTemplate InventoryDefnTemplate { get; set; }
        public DataTemplate NotSupportedTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return LocalSelectTemplate(item);
        }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return LocalSelectTemplate(item);
        }
        private DataTemplate LocalSelectTemplate(object item)
        {
            System.Diagnostics.Debug.WriteLine($"Selecting a template for {item?.GetType().Name ?? "Null Object"}");

            switch(item)
            {
                case VendorDefn vendorDefn:
                    return VendorDefnTemplate;
                    break;
                    case SpoolDefn poolDefn:
                    return SpoolDefnTemplate;
                    break;
                case InventorySpool inventorySpool:
                    return InventoryDefnTemplate;
                    default:
                    return NotSupportedTemplate;

            }    
        }
    }
}
