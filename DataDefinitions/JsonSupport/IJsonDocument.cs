using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DataDefinitions.Models;

namespace DataDefinitions.JsonSupport
{
    /// <summary>
    /// Contains objects/collections that support the Filament database in json format
    /// </summary>
    /// <remarks>
    /// Most of the 'objects' are contained within other items.  So the number of 'collections' is minimal.  Settings, Vendors, and Filaments
    /// </remarks>
    public interface IDocument
    {
        ICollection<VendorDefn> Vendors { get; }
        ICollection<FilamentDefn> Filaments { get; }
        ICollection<Setting> Settings { get; }
        CounterProvider Counters { get; }
    }
}
