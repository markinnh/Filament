using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.LiteDBSupport
{
    public interface ILiteDBDAL:IStandardCrudActions
    {
        ObservableCollection<FilamentDefn> Filaments { get; }
        CollateTagsCollection<VendorDefn> Vendors { get; }
        CollateTagsCollection<NoteDefn> Notes { get; }
        List<Setting> Settings { get; }
        ObservableCollection<PrintSettingDefn> PrintSettings { get; }
    }
}
