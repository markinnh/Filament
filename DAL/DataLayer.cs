using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataLayer : INotifyPropertyChanged
    {
        protected ObservableCollection<VendorDefn> Vendors { get; set; }
        protected ObservableCollection<FilamentDefn> Filaments { get; set; }

        protected ObservableCollection<PrintSettingDefn> PrintSettings { get; set; }

        protected List<Setting> Settings { get; set; }
        public Func<VendorDefn, bool> FilterVendor { get; set; }
        public Func<FilamentDefn, bool> FilterFilament { get; set; }

        public IEnumerable<VendorDefn> VendorList { get => FilterVendor == null ? Vendors : Vendors.Where(FilterVendor); }
        public IEnumerable<FilamentDefn> FilamentList { get => FilterFilament == null ? Filaments : Filaments.Where(FilterFilament); }

        public IEnumerable<PrintSettingDefn> PrintSettingsList { get => PrintSettings; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable<Setting> GetFilteredSettings(Func<Setting, bool> func) => Settings.Where(x => func(x));
        public Setting? GetSingleSetting(Func<Setting, bool> func) => GetFilteredSettings(func).SingleOrDefault();
        public IEnumerable<VendorDefn> GetFilteredVendors(Func<VendorDefn, bool> func) => Vendors.Where(v => func(v));

        public IEnumerable<FilamentDefn> GetFilteredFilaments(Func<FilamentDefn, bool> func) => Filaments.Where(f => func(f));

        public IEnumerable<PrintSettingDefn> GetFilteredPrintSettingDefns(Func<PrintSettingDefn, bool> func) => PrintSettings.Where(x => func(x));
        public PrintSettingDefn? GetSinglePrintSettingDefn(Func<PrintSettingDefn, bool> func) => PrintSettings?.FirstOrDefault(ps => func(ps));

        #region Add Methods
        public void Add(VendorDefn vendor)
        {
            Vendors.Add(vendor);
            OnPropertyChanged(nameof(FilamentList));
        }
        public void Add(FilamentDefn filament)
        {
            Filaments.Add(filament);
            OnPropertyChanged(nameof(FilamentList));
        }
        public void Add(PrintSettingDefn printSetting)
        {
            PrintSettings.Add(printSetting);
            OnPropertyChanged(nameof(PrintSettingsList));
        }
        public void Add(DataDefinitions.DatabaseObject databaseObject)
        {
            if (databaseObject is VendorDefn vendor)
                Add(vendor);
            else if (databaseObject is FilamentDefn filament)
                Add(filament);
            else if (databaseObject is Setting setting)
                Add(setting);
            else if(databaseObject is PrintSettingDefn printSetting)
                Add(printSetting);
        }
        

        public void Add(Setting setting)
        {
            Settings.Add(setting);
        }
        #endregion
        public void Remove(PrintSettingDefn settingDefn)
        {
            PrintSettings?.Remove(settingDefn);
            OnPropertyChanged(nameof(PrintSettingsList));
        }
        public void Remove(FilamentDefn filamentDefn)=>Filaments?.Remove(filamentDefn);
        public void Remove(Setting setting)=> Settings?.Remove(setting);
        public void Remove(VendorDefn vendorDefn)=> Vendors?.Remove(vendorDefn);
        public void Remove(DataDefinitions.DatabaseObject databaseObject)
        {
            if(databaseObject is VendorDefn vendor)
                Remove(vendor);
            else if( databaseObject is FilamentDefn filament)
                Remove(filament);
            else if(databaseObject is Setting setting)
                Remove(setting);
            else if(databaseObject is PrintSettingDefn settingDefn)
                Remove(settingDefn);
        }
        public DataLayer()
        {
            VendorDefn.SetDataOperationsState(true);
            FilamentDefn.SetDataOperationsState(true);
            PrintSettingDefn.SetDataOperationsState(true);

            if (Abstraction.GetAllVendors() is List<VendorDefn> v)
                Vendors = new ObservableCollection<VendorDefn>(v);


            FilamentDefn.SetDataOperationsState(true);
            if (Abstraction.GetAllFilaments() is List<FilamentDefn> fi)
                Filaments = new ObservableCollection<FilamentDefn>(fi);

            if(Abstraction.GetAllPrintSettingDefns() is List<PrintSettingDefn> ps)
                PrintSettings= new ObservableCollection<PrintSettingDefn>(ps);
            

            if (Vendors != null && Filaments != null)
            {
                foreach (var vend in Vendors)
                {
                    vend.WatchContained();
                    
                    foreach(var settingsConfig in vend.VendorSettings) { 
                        settingsConfig.WatchContained();
                        settingsConfig.Link(Filaments);
                        settingsConfig.Link(PrintSettings);
                    }
                    foreach (var sp in vend.SpoolDefns)
                    {
                        
                        foreach (var inv in sp.Inventory)
                            inv.Link(Filaments);
                    }
                }

                foreach (var filament in Filaments)
                    filament.InitNotificationHandler();
            }
            VendorDefn.SetDataOperationsState(false);
            FilamentDefn.SetDataOperationsState(false);
            PrintSettingDefn.SetDataOperationsState(false);
            Setting.InDataOps = true;

            if (Abstraction.GetAllSettings() is List<Setting> settings)
                Settings = settings;

            Setting.InDataOps = false;
        }
        ~DataLayer()
        {
            if (Filaments != null)
                foreach (var filament in Filaments)
                    filament.ReleaseNotificationHandler();
            if (PropertyChanged?.GetInvocationList().Cast<PropertyChangedEventHandler>() is IEnumerable<PropertyChangedEventHandler> handlers)
            {
                foreach (PropertyChangedEventHandler handler in handlers)
                    PropertyChanged -= handler;
            }
        }
    }
}
