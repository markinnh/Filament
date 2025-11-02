using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions.Models;
using LiteDB;
using System.Diagnostics;

namespace DataDefinitions.LiteDBSupport;

public partial class LiteDBDal
{
#if SUPPORTDELETE
    public void Remove(FilamentDefn filament)
    {
        Debug.Assert(!filament.IsIntrinsic, "Can't delete intrinsic filaments");
        if (filament.IsIntrinsic)
            throw new InvalidOperationException("Intrinsic filaments cannot be deleted.  This operation shouldn't have happened!");

        using LiteDatabase database = new LiteDatabase(ConnectionString);
        database.GetCollection<FilamentDefn>(filamentTableName).Delete(filament.FilamentDefnId);
    }
    //public void Remove(InventorySpool inventorySpool)
    //{
    //    using LiteDatabase db = new LiteDatabase(ConnectionString);
    //    db.GetCollection<InventorySpool>(inventoryTableName).Delete(inventorySpool.KeyID);
    //}
    public void Remove(FilamentColor filamentColor) 
    {
        using LiteDatabase db = new LiteDatabase(ConnectionString);
        db.GetCollection<FilamentColor>(filamentColorsTableName).Delete(filamentColor.FilamentColorId);
    }
    public void Remove(VendorDefn defn)
    {
        using LiteDatabase database = new LiteDatabase(ConnectionString);
        Vendors.Remove(defn);
        database.GetCollection<VendorDefn>(vendorTableName).Delete(defn.VendorDefnId);
    }
#endif
    public void Import(FilamentDefn filamentDefn)
    {
        if (filamentDefn == null)
            throw new ArgumentNullException(nameof(filamentDefn), "The filament definition to import was not passed.");
        var vName = filamentDefn.VendorName;
        if (Vendors.All(v => v.Name != vName))
            throw new InvalidOperationException($"The vendor '{vName}' for the filament definition to import is not defined in the vendors table.  Unable to import the Filament Definition, perhaps import {vName} Vendor Definition first.");

        // TODO: find an existing material definition that matches this one, or add it if it doesn't exist.
        var material = Materials.FirstOrDefault(m => m.Name == filamentDefn.Material.Name && m.ShortName == filamentDefn.Material.ShortName);
        if (material != null)
        {
            filamentDefn.MaterialId = material.MaterialId;
            filamentDefn.Material = material;
        }
        else
        {
            Materials.Add(filamentDefn.Material);
            Add(filamentDefn.Material);
            filamentDefn.MaterialId = filamentDefn.Material.MaterialId;
        }
        var referenceVendor = Vendors.First(v => v.Name == vName);
        filamentDefn.VendorID = referenceVendor.VendorDefnId;
        //filamentDefn.Vendor= referenceVendor;
        Filaments.Add(filamentDefn);
        Add(filamentDefn);
    }
    public void Import(FilamentColor color)
    {
        if (color == null)
            throw new ArgumentNullException(nameof(color), "The filament color to import was not passed.");
        if (FilamentColors.Any(f => f.ColorRGB == color.ColorRGB && f.VendorColorName == color.VendorColorName))
            throw new InvalidOperationException("The filament color to import is already defined in the filament colors table.");

        FilamentColors.Add(color);
        Add(color);
    }
    public void Import(VendorDefn vendor)
    {
        if (vendor == null)
            throw new ArgumentNullException(nameof(vendor), "The vendor to import was not passed.");
        if (Vendors.Any(v => v.Name == vendor.Name))
            throw new InvalidOperationException("The vendor to import is already defined in the vendors table.");
        Vendors.Add(vendor);
        Add(vendor);
    }
}
