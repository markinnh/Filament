using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.LiteDBSupport;

public interface ILiteDBDAL:IStandardCrudActions
{
    ObservableCollection<FilamentDefn> Filaments { get; }
    CollateTagsCollection<VendorDefn> Vendors { get; }
    CollateTagsCollection<NoteDefn> Notes { get; }
    List<Setting> Settings { get; }
    ObservableCollection<PrintSettingDefn> PrintSettings { get; }
    void SmartAdd(DataObject databaseObject);
#if SUPPORTDELETE
    void Remove(VendorDefn vendor);
    void Remove(FilamentColor color);
    void Remove(FilamentDefn filament);
#endif
#if DEBUG // Import Support
    void Import(VendorDefn vendor);
    void Import(FilamentColor color);
    void Import(FilamentDefn filament);

#endif
}
