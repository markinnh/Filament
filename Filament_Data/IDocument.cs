using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Filament_Data
{
    public interface IDocument
    {
        CounterProvider Counters { get; }
        DefinedVendors Vendors { get; }
        DefinedFilaments Filaments { get; }
        DefinedSpools Spools { get; }
        ObservableCollection<InventorySpool> Inventory { get; }
        ObservableCollection<PrintJobDefn> Prints { get; }
        //DictionaryContainer<string,FilamentDefn> Filaments { get; }
        bool IsModified { get; }
    }
    public interface IChangeTrackItem
    {
        bool IsModified { get; }
    }

    /// <summary>
    /// restore linked data after deserializing from json, to allow for smaller footprint
    /// </summary>
    public interface ILinkedItem:IChangeTrackItem
    {
        IDocument Document { get; }
        void EstablishLink(IDocument document);
    }
    public interface IParentItem<ParentType>
    {
        ParentType Parent { get; set; }
    }
}
