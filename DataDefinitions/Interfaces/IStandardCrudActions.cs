using DataDefinitions.Models;

namespace DataDefinitions.Interfaces;
public interface IStdDatabaseActions
{
    void SeedDatabase();
}

public interface IStandardCrudActions:IStdDatabaseActions
{
    void Add(FilamentDefn filament);
    void Add(PrintSettingDefn printSettingDefn);
    void Add(Setting setting);
    void Add(VendorDefn vendor);
    void Remove(FilamentColor color);
    void Remove(FilamentDefn filament);
    void Remove(VendorDefn vendor);
    void Update(FilamentDefn filament);
    void Update(PrintSettingDefn printSettingDefn);
    void Update(Setting setting);
    void Update(VendorDefn vendor);
}