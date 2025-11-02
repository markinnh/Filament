using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Filament.WPF6.Helpers;

internal class ByVendorSelector:DataTemplateSelector
{
    public DataTemplate? NewItemTemplate { get; set; }
    public DataTemplate? VendorDefitionTemplate { get; set; }
    public DataTemplate? SpoolDefinitionTemplate { get; set;}
    public DataTemplate? InventorySpoolTemplate { get; set;}
    public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
    {
        if (item is null)
            return base.SelectTemplate(item, container);
        else if (item is DataDefinitions.Models.VendorDefn)
            return VendorDefitionTemplate!;
        else if (item is DataDefinitions.Models.SpoolDefn)
            return SpoolDefinitionTemplate!;
        else if (item is DataDefinitions.Models.InventorySpool)
            return InventorySpoolTemplate!;
        else
            return NewItemTemplate!;
    }
}
