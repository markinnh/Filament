using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext
{
    public class DataLayer : INotifyPropertyChanged
    {
        protected ObservableCollection<VendorDefn> Vendors { get; set; }
        protected ObservableCollection<FilamentDefn> Filaments { get; set; }
        protected List<Setting> Settings { get; set; }
        public Func<VendorDefn, bool> FilterVendor { get; set; }
        public Func<FilamentDefn, bool> FilterFilament { get; set; }

        public IEnumerable<VendorDefn> VendorList { get => FilterVendor == null ? Vendors : Vendors.Where(FilterVendor); }
        public IEnumerable<FilamentDefn> FilamentList { get => FilterFilament == null ? Filaments : Filaments.Where(FilterFilament); }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable<Setting> GetFilteredSettings(Func<Setting, bool> func) => Settings.Where(x => func(x));
        public Setting GetSingleSetting(Func<Setting, bool> func) => GetFilteredSettings(func).SingleOrDefault();
        public IEnumerable<VendorDefn> GetFilteredVendors(Func<VendorDefn, bool> func) => Vendors.Where(v => func(v));

        public IEnumerable<FilamentDefn> GetFilteredFilaments(Func<FilamentDefn, bool> func) => Filaments.Where(f => func(f));

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
        public void Add(Setting setting)
        {
            Settings.Add(setting);
        }
        public DataLayer()
        {
            VendorDefn.SetDataOperationsState(true);

            if (FilamentContext.GetAllVendors() is List<VendorDefn> v)
                Vendors = new ObservableCollection<VendorDefn>(v);


            FilamentDefn.SetDataOperationsState(true);
            if (FilamentContext.GetAllFilaments() is List<FilamentDefn> fi)
                Filaments = new ObservableCollection<FilamentDefn>(fi);



            if (Vendors != null && Filaments != null)
            {
                foreach (var vend in Vendors)
                    foreach (var sp in vend.SpoolDefns)
                        foreach (var inv in sp.Inventory)
                            inv.Link(Filaments);

                foreach (var filament in Filaments)
                    filament.InitNotificationHandler();
            }
            VendorDefn.SetDataOperationsState(false);
            FilamentDefn.SetDataOperationsState(false);

            Setting.InDataOps = true;

            if (FilamentContext.GetAllSettings() is List<Setting> settings)
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
