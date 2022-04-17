

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Diagnostics.Debug;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DataDefinitions.Models
{
    // TODO: Develop a UI for VendorDefn; Add, Delete, Update
    // TODO: Develop a DataObject class the sit between data classes and Observable since the data is going to be disconnected from the DbContext
    public class VendorDefn : DatabaseObject, IDataErrorInfo, IEditableObject
    {
        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the FilamentDefn objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        public override bool InDataOperations => InDataOps;
        public override bool IsModified { get => base.IsModified || SpoolDefns?.Count(sd => sd.IsModified) > 0 || VendorSettings?.Count(vs => vs.IsModified) > 0; set => base.IsModified = value; }
        protected override bool HasContainedItems => true;
        public override bool InDatabase => vendorID != default;

        public override bool IsValid => !string.IsNullOrEmpty(name);
        //public const string _3DSolutechName = "3D Solutech";
        //public const string HatchBoxName = "HatchBox";
        //public const string SunluName = "Sunlu";
        //public const string DefaultVendorName = "Generic";
        private int vendorID;
        /// <summary>
        /// Gets or sets the vendor identifier.
        /// </summary>
        /// <value>
        /// The vendor identifier.
        /// </value>
        public int VendorDefnId
        {
            get => vendorID;
            set => Set<int>(ref vendorID, value);
        }

        private string name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name
        {
            get => name;
            set => Set<string>(ref name, value);
        }

        private bool foundOnAmazon;
        /// <summary>
        /// Gets or sets a value indicating whether [found on amazon].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [found on amazon]; otherwise, <c>false</c>.
        /// </value>
        public bool FoundOnAmazon
        {
            get => foundOnAmazon;
            set => Set<bool>(ref foundOnAmazon, value);
        }

        private string webUrl;
        /// <summary>
        /// Gets or sets the web URL.
        /// </summary>
        /// <value>
        /// The web URL.
        /// </value>
        public string WebUrl
        {
            get => webUrl;
            set => Set<string>(ref webUrl, value);
        }

        public Uri WebUri => !string.IsNullOrEmpty(webUrl) ? new Uri(webUrl, UriKind.Absolute) : new Uri("https://www.google.com");
        private bool stopUsing;
        /// <summary>
        /// Gets or sets a value indicating whether to stop using.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop using]; otherwise, <c>false</c>.
        /// </value>
        public bool StopUsing
        {
            get => stopUsing;
            set => Set<bool>(ref stopUsing, value);
        }
        [NotMapped]
        public bool CollectionNotInitialized { get => SpoolDefns == null; }
        public ObservableCollection<SpoolDefn> SpoolDefns { get; set; }
        public ObservableCollection<VendorSettingsConfig> VendorSettings { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name) && string.IsNullOrEmpty(Name))
                    return $"{nameof(Name)} is a required entry";
                else
                    return string.Empty;
            }
        }
        public VendorDefn()
        {
            Name = "Undefined";
            InDataOpsChanged += VendorDefn_InDataOpsChanged;
            SpoolDefns = new ObservableCollection<SpoolDefn>();
            if (SpoolDefns is ObservableCollection<SpoolDefn> defns)
                defns.CollectionChanged += Defns_CollectionChanged;

            if (new ObservableCollection<VendorSettingsConfig>() is ObservableCollection<VendorSettingsConfig> vendorPrintSettings)
            {
                VendorSettings = vendorPrintSettings;
                vendorPrintSettings.CollectionChanged += PrintSettings_CollectionChanged;
            }
        }

        private void PrintSettings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
            {
                foreach (VendorSettingsConfig settingDefn in e.NewItems)
                {
                    settingDefn.Subscribe(WatchContainedHandler);
                    settingDefn.WatchContained();
                    settingDefn.VendorDefnId = vendorID;
                    settingDefn.VendorDefn = this;
                }
                if (!InDataOperations)
                    IsModified = true;
            }
            else
                throw new NotImplementedException($"Actions not implemented for {e.Action} in {nameof(PrintSettings_CollectionChanged)}");
        }

        private void Defns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    if (item is SpoolDefn spoolDefn)
                    {
                        spoolDefn.Subscribe(WatchContainedHandler);
                        spoolDefn.WatchContained();
                        spoolDefn.Vendor = this;
                        spoolDefn.VendorDefnId = VendorDefnId;
                    }
            if (!InDataOperations)
                IsModified = true;
            //throw new NotImplementedException();
        }

        private void SpoolDefn_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WriteLine($"Contained property changed for : {e.PropertyName}");
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
            //throw new NotImplementedException();
        }

        ~VendorDefn()
        {
            InDataOpsChanged -= VendorDefn_InDataOpsChanged;
        }
        private void VendorDefn_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }
        public override void WatchContained()
        {
            //Subscribe(WatchContainedHandler);

            foreach (var spool in SpoolDefns)
            {
                spool.Subscribe(WatchContainedHandler);
                spool.WatchContained();
            }
            foreach (var settingCfg in VendorSettings)
            {
                settingCfg.Subscribe(WatchContainedHandler);
                settingCfg.WatchContained();
            }
        }
        public override void UnWatchContained()
        {
            foreach (var spool in SpoolDefns)
            {
                spool.Unsubscribe(WatchContainedHandler);
                spool.UnWatchContained();
            }
            foreach (var settingCfg in VendorSettings)
            {
                settingCfg.Unsubscribe(WatchContainedHandler);
                settingCfg.UnWatchContained();
            }
        }
        public override string UIHintAddType() => typeof(SpoolDefn).GetCustomAttribute<UIHintsAttribute>()?.AddType ?? String.Empty;



        /// <summary>
        /// Creates the vendor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="foundOnAmazon">if set to <c>true</c> [found on amazon].</param>
        /// <param name="url">The URL.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static VendorDefn CreateVendor(string name, bool foundOnAmazon, string url)
        {
            var result = new VendorDefn()
            {
                Name = name,
                FoundOnAmazon = foundOnAmazon,
                WebUrl = url
            };
            return result;
        }
        internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        {
            if (InDatabase)
            {
                context.SetDataItemsState<SpoolDefn>(SpoolDefns.Where(sd => Modified(sd)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                context.SetDataItemsState<SpoolDefn>(SpoolDefns.Where(sd => Added(sd)), Microsoft.EntityFrameworkCore.EntityState.Added);
                context.SetDataItemsState(VendorSettings.Where(vs => Modified(vs)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                context.SetDataItemsState(VendorSettings.Where(vs => Added(vs)), Microsoft.EntityFrameworkCore.EntityState.Added);
            }
            else
            {
                context.SetDataItemsState<DatabaseObject>(SpoolDefns.Where(Added), Microsoft.EntityFrameworkCore.EntityState.Added);
                context.SetDataItemsState(VendorSettings.Where<DatabaseObject>(Added), Microsoft.EntityFrameworkCore.EntityState.Added);
            }
            foreach (VendorSettingsConfig settingsConfig in VendorSettings)
                settingsConfig.UpdateContainedItemEntryState<TContext>(context);
        }
        public static void SetDataOperationsState(bool state)
        {
            InDataOps = state;
            SpoolDefn.InDataOps = state;
            InventorySpool.InDataOps = state;
            DepthMeasurement.InDataOps = state;
            VendorSettingsConfig.InDataOps = state;
            ConfigItem.InDataOps = state;
        }
        public override void SetContainedModifiedState(bool state)
        {

            if (SpoolDefns != null)
                foreach (var spec in SpoolDefns)
                {
                    spec.IsModified = state;
                    foreach (var inv in spec.Inventory)
                    {
                        inv.IsModified = state;
                        foreach (var dm in inv.DepthMeasurements)
                            dm.IsModified = state;
                    }
                }
            if(VendorSettings!=null)
                foreach (var settingsConfig in VendorSettings)
                    settingsConfig.SetContainedModifiedState(state);

            IsModified = state;
            OnPropertyChanged(nameof(IsModified));
        }
        #region IEditableObject Implementation
        struct BackupData
        {
            public string Name { get; set; }
            public string WebUrl { get; set; }
            public bool StopUsing { get; set; }
            public bool FoundOnAmazon { get; set; }

            internal BackupData(string name, string webUrl, bool stopUsing, bool foundOnAmazon)
            {
                Name = name;
                WebUrl = webUrl;
                StopUsing = stopUsing;
                FoundOnAmazon = foundOnAmazon;
            }
        }
        BackupData backupData;
        void IEditableObject.BeginEdit()
        {
            if (!InEdit)
            {
                backupData = new BackupData(name, webUrl, stopUsing, foundOnAmazon);
                InEdit = true;
            }
            //throw new NotImplementedException();
        }

        void IEditableObject.CancelEdit()
        {
            if (InEdit)
            {
                name = backupData.Name;
                webUrl = backupData.WebUrl;
                StopUsing = backupData.StopUsing;
                foundOnAmazon = backupData.FoundOnAmazon;
                backupData = default(BackupData);
                InEdit = false;
                SetContainedModifiedState(false);
            }
            //throw new NotImplementedException();
        }

        void IEditableObject.EndEdit()
        {
            if (InEdit)
            {
                backupData = default(BackupData);
                InEdit = false;
            }
            //throw new NotImplementedException();
        }
        #endregion
    }
}
