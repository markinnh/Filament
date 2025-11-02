using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExportSelect.Helpers;
public partial class SelectWrapper<TSelect>:ObservableObject where TSelect : DataDefinitions.DataObject, new()
{
    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private TSelect _item;
    public SelectWrapper(TSelect select,bool isSelected=false)
    {
        Item = select;
        IsSelected = isSelected;
    }
}
public class SelectFilamentColorWrapper : SelectWrapper<DataDefinitions.Models.FilamentColor>
{
    public SelectFilamentColorWrapper(DataDefinitions.Models.FilamentColor item, bool isSelected = false):base(item,isSelected)
    {
    }
}
public class SelectVendorDefnWrapper : SelectWrapper<DataDefinitions.Models.VendorDefn>
{
    public SelectVendorDefnWrapper(DataDefinitions.Models.VendorDefn item, bool isSelected = false) : base(item, isSelected)
    {
    }
}
public class SelectMaterialWrapper : SelectWrapper<DataDefinitions.Models.Material>
{
    public SelectMaterialWrapper(DataDefinitions.Models.Material item, bool isSelected = false) : base(item, isSelected)
    {
    }
}
public class  SelectFilamentDefnWrapper:SelectWrapper<DataDefinitions.Models.FilamentDefn>
{
    public SelectFilamentDefnWrapper(DataDefinitions.Models.FilamentDefn defn,bool isSelected = false) : base(defn, isSelected) { }
}