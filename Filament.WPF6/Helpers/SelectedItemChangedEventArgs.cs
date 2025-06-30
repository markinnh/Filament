using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions;

namespace Filament.WPF6.Helpers;
public class SelectedItemChangedEventArgs:EventArgs
{
    public SelectedItemChangedEventArgs(DataObject selectedItem)
    {
        SelectedItem = selectedItem;
    }

    public DataObject SelectedItem
    {
        get;
        set;
    }   
}
