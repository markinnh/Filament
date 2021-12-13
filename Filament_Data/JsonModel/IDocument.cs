using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Filament_Data.JsonModel
{
    public interface IDocument:ITrackModified
    {
        CounterProvider Counters { get; }
        DefinedVendors Vendors { get; }
        DefinedFilaments Filaments { get; }
        DefinedSpools Spools { get; }
        DocumentLinkedCollection<InventorySpool> Inventory { get; }
        ObservableCollection<PrintJobDefn> PrintJobs { get; }
        //DictionaryContainer<string,FilamentDefn> Filaments { get; }
    }
    public interface ITrackModified
    {
        bool IsModified { get; }
    }
    internal interface IUpdateModified
    {
        void SuccessfullySaved();
    }
    /// <summary>
    /// restore data links to dependent objects after deserializing from json, to allow for smaller footprint
    /// </summary>
    public interface ILinked:ITrackModified
    {
        IDocument Document { get; }
        void EstablishLink(IDocument document);
    }
    /// <summary>
    /// maintain a link to a parent, allows children to access parent properties
    /// </summary>
    /// <typeparam name="ParentType">The type of the arent type.</typeparam>
    public interface IParentRef<ParentType>
    {
        ParentType Parent { get; set; }
    }

}
