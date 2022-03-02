using DataDefinitions.Models;
using dd = DataDefinitions;
using sql = DataContext;
using sqlite = SqliteContext;
namespace DAL
{
    public enum DataAccessSelector:int
    {
        SqlServer=0x1000,
        Sqlite
    }
    public class Abstraction
    {
        public static DataAccessSelector DataAccessSelector { get; set; } = DataAccessSelector.Sqlite;
        public static void AddAll(params object[] items)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                dd.BaseFilamentContext.AddAll<sql.FilamentContext>(items);
            else
                dd.BaseFilamentContext.AddAll<sqlite.FilamentContext>(items);
        }
        public static List<VendorDefn> GetAllVendors()
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetAllVendors<sql.FilamentContext>();
            else
                return dd.BaseFilamentContext.GetAllVendors<sqlite.FilamentContext>();
        }
        public static List<VendorDefn> GetSomeVendors(Func<VendorDefn, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetSomeVendors<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetSomeVendors<sqlite.FilamentContext>(func);
        }
        public static VendorDefn GetVendor(Func<VendorDefn, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetVendor<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetVendor<sqlite.FilamentContext>(func);
        }
        public static List<FilamentDefn> GetAllFilaments()
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetAllFilaments<sql.FilamentContext>();
            else
                return dd.BaseFilamentContext.GetAllFilaments<sqlite.FilamentContext>();
        }
        public static List<FilamentDefn> GetFilaments(Func<FilamentDefn, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetFilaments<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetFilaments<sqlite.FilamentContext>(func);
        }
        public static FilamentDefn GetFilament(Func<FilamentDefn, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetFilament<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetFilament<sqlite.FilamentContext>(func);
        }
        public static List<Setting> GetAllSettings()
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetAllSettings<sql.FilamentContext>();
            else
                return dd.BaseFilamentContext.GetAllSettings<sqlite.FilamentContext>();
        }
        public static List<Setting> GetSettings(Func<Setting, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetSettings<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetSettings<sqlite.FilamentContext>(func);
        }

        public static Setting GetSetting(Func<Setting, bool> func)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                return dd.BaseFilamentContext.GetSetting<sql.FilamentContext>(func);
            else
                return dd.BaseFilamentContext.GetSetting<sqlite.FilamentContext>(func);
        }
        public static void UpdateItem(dd.DatabaseObject databaseObject)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                databaseObject.UpdateItem<sql.FilamentContext>();
            else
                databaseObject.UpdateItem<sqlite.FilamentContext>();
        }
        public static void Remove(dd.DatabaseObject databaseObject)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                databaseObject.Delete<sql.FilamentContext>();
            else
                databaseObject.Delete<sqlite.FilamentContext>();
        }
        public static void SeedData(ref bool NeedMigration)
        {
            if (DataAccessSelector == DataAccessSelector.SqlServer)
                dd.DataSeed.Seed<sql.FilamentContext>(ref NeedMigration);
            else
                dd.DataSeed.Seed<sqlite.FilamentContext>(ref NeedMigration);
        }
        public static void VerifySeed()
        {
            if(DataAccessSelector== DataAccessSelector.SqlServer)
                dd.DataSeed.VerifySeed<sql.FilamentContext>();
            else
                dd.DataSeed.VerifySeed<sqlite.FilamentContext>();
        }
    }
}