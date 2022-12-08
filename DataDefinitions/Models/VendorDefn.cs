

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Text;
using static System.Diagnostics.Debug;
using System.Linq;

using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using LiteDB;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;

namespace DataDefinitions.Models
{
    // TODO: Develop a UI for VendorDefn; Add, Delete, Update
    // TODO: Develop a DataObject class the sit between data classes and Observable since the data is going to be disconnected from the DbContext
    public class VendorDefn : TaggedDatabaseObject, IDataErrorInfo, IEditableObject, ITrackUsable, IReferenceUsage<FilamentDefn>
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
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public override bool InDataOperations => InDataOps;
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public override bool IsModified { get => base.IsModified || SpoolDefns?.Count(sd => sd.IsModified) > 0 || VendorSettings?.Count(vs => vs.IsModified) > 0; set => base.IsModified = value; }
        protected override bool HasContainedItems => true;
        //public override bool InDatabase => vendorID != default;
        [JsonIgnore, XmlIgnore, BsonIgnore]
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
        [XmlAttribute("ID"), JsonPropertyName("ID")]
        public int VendorDefnId
        {
            get => vendorID;
            set => Set<int>(ref vendorID, value);
        }
        internal override int KeyID
        {
            get => vendorID;
            set
            {
                Set(ref vendorID, value);
                foreach (var item in VendorSettings)
                    item.VendorDefnId = value;
                foreach (var item in SpoolDefns)
                    item.VendorDefnId = value;
            }
        }

        private string name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlAttribute("name")]
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
        [XmlAttribute("onAmazon")]
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
        [XmlAttribute("link")]
        public string WebUrl
        {
            get => webUrl;
            set => Set<string>(ref webUrl, value);
        }
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public Uri WebUri => !string.IsNullOrEmpty(webUrl) ? new Uri(webUrl, UriKind.Absolute) : new Uri("https://www.google.com");
        private bool stopUsing;
        /// <summary>
        /// Gets or sets a value indicating whether to stop using.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop using]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("notUsed")]
        public bool StopUsing
        {
            get => stopUsing;
            set => Set<bool>(ref stopUsing, value);
        }
        [JsonIgnore, BsonIgnore]
        public bool CollectionNotInitialized { get => SpoolDefns == null; }
        public ObservableCollection<SpoolDefn> SpoolDefns { get; set; }
        public ObservableCollection<VendorPrintSettingsConfig> VendorSettings { get; set; }
        [JsonIgnore]
        public string Error => null;
        //private string tags;
        //public string Tags { get => tags; set => Set(ref tags, value); }

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
            //if (SpoolDefns is ObservableCollection<SpoolDefn> defns)
            //    defns.CollectionChanged += Defns_CollectionChanged;

            VendorSettings = new ObservableCollection<VendorPrintSettingsConfig>();
            //VendorSettings.CollectionChanged += PrintSettings_CollectionChanged;
            InitEventHandlers();
        }

        private void PrintSettings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
            {
                foreach (VendorPrintSettingsConfig settingDefn in e.NewItems)
                {
                    settingDefn.Subscribe(WatchContainedHandler);
                    settingDefn.WatchContained();
                    settingDefn.VendorDefnId = vendorID;
                    settingDefn.VendorDefn = this;
                    //settingDefn.EstablishLink(Document);
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
                        spoolDefn.Parent = this;
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

        protected override void SaveToJsonDatabase()
        {
            //if (Document != null)
            //    Document.Add(this);
        }

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
        //internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        //{
        //    if (InDatabase)
        //    {
        //        context.SetDataItemsState<SpoolDefn>(SpoolDefns.Where(sd => Modified(sd)), Microsoft.EntityFrameworkCore.EntityState.Modified);
        //        context.SetDataItemsState<SpoolDefn>(SpoolDefns.Where(sd => Added(sd)), Microsoft.EntityFrameworkCore.EntityState.Added);
        //        context.SetDataItemsState(VendorSettings.Where(vs => Modified(vs)), Microsoft.EntityFrameworkCore.EntityState.Modified);
        //        context.SetDataItemsState(VendorSettings.Where(vs => Added(vs)), Microsoft.EntityFrameworkCore.EntityState.Added);
        //    }
        //    else
        //    {
        //        context.SetDataItemsState(SpoolDefns.Where(Added), Microsoft.EntityFrameworkCore.EntityState.Added);
        //        context.SetDataItemsState(VendorSettings.Where(Added), Microsoft.EntityFrameworkCore.EntityState.Added);
        //    }
        //    foreach (VendorSettingsConfig settingsConfig in VendorSettings)
        //        settingsConfig.UpdateContainedItemEntryState<TContext>(context);
        //}
        public static void SetDataOperationsState(bool state)
        {
            InDataOps = state;
            SpoolDefn.InDataOps = state;
            InventorySpool.InDataOps = state;
            DepthMeasurement.InDataOps = state;
            VendorPrintSettingsConfig.InDataOps = state;
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
            if (VendorSettings != null)
                foreach (var settingsConfig in VendorSettings)
                    settingsConfig.SetContainedModifiedState(state);

            IsModified = state;
            OnPropertyChanged(nameof(IsModified));
        }

        public override void LinkChildren<ParentType>(ParentType parent)
        {
            //Assert(Document != null, "Document is not initialized before calling this method.");

            //spoolDefn.LinkChildren<VendorDefn>(this);
        }
        public void LinkInventoryToFilaments(IEnumerable<FilamentDefn> filaments)
        {
            foreach (var spoolDefn in SpoolDefns)
                foreach (var inv in spoolDefn.Inventory)
                    inv.FilamentDefn = filaments.First(f => f.FilamentDefnId == inv.FilamentDefnId);

            foreach (var vSetting in VendorSettings)
                vSetting.FilamentDefn = filaments.First(f => f.FilamentDefnId == vSetting.FilamentDefnId);
        }
        public void LinkVendorSettingsToPrintDefns(IEnumerable<PrintSettingDefn> printSettings)
        {
            foreach (var vSetting in VendorSettings)
                foreach (var cfgItem in vSetting.ConfigItems)
                    cfgItem.PrintSettingDefn = printSettings.First(ps => ps.PrintSettingDefnId == cfgItem.PrintSettingDefnId);
        }
        //public override void EstablishLink(IJsonFilamentDocument document)
        //{
        //    base.EstablishLink(document);
        //    InitEventHandlers();

        //    foreach (var printSetting in VendorSettings)
        //    {
        //        printSetting.Link(document.Filaments);
        //        printSetting.Link(document.PrintSettingsDefn);
        //        //printSetting.EstablishLink(document);
        //    }
        //    foreach (var spoolDefn in SpoolDefns)
        //    {
        //        //spoolDefn.EstablishLink(document);
        //        foreach (var inventory in spoolDefn.Inventory)
        //            inventory.Link(document.Filaments);
        //    }
        //}
        public override void PostDataRetrieveActions()
        {
            InitEventHandlers();
            foreach (SpoolDefn spoolDefn in SpoolDefns)
            {
                spoolDefn.LinkToParent(this);
                spoolDefn.PostDataRetrieveActions();
            }
        }
        private void InitEventHandlers()
        {
            SpoolDefns.CollectionChanged += Defns_CollectionChanged;
            VendorSettings.CollectionChanged += PrintSettings_CollectionChanged;
        }

        internal override void AssignKey(int myId)
        {
            base.AssignKey(myId);
            foreach (var spoolDefn in SpoolDefns)
            {
                spoolDefn.VendorDefnId = myId;
                //                spoolDefn.AssignKey(Document.Counters.NextID(spoolDefn));
            }
        }
        /// <summary>
        /// Called from UpdateItem, used to update contained items, primarily for updating contained item Keys
        /// </summary>
        internal override void UpdateContainedItems()
        {
            foreach (var spoolDefn in SpoolDefns)
            {
                //if (!spoolDefn.InDatabase && spoolDefn.IsValid)
                //    spoolDefn.AssignKey(Document.Counters.NextID(spoolDefn));

                //spoolDefn.UpdateContainedItems();
            }
            foreach (var setting in VendorSettings)
            {
                //if (setting.IsValid)
                //    setting.AssignKey(Document.Counters.NextID(setting));
                //setting.UpdateContainedItems();
            }
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

        public IEnumerable<DataObject> GetReferences(FilamentDefn defn)
        {
            foreach (var spool in SpoolDefns)
                foreach (var inv in spool.Inventory)
                    if (inv.FilamentDefnId == defn.FilamentDefnId)
                        yield return inv;
            foreach (var setting in VendorSettings)
                if (setting.FilamentDefnId == defn.FilamentDefnId)
                    yield return setting;
            //throw new NotImplementedException();
        }

        //public IEnumerable<string> GetTags()
        //{
        //    if (!string.IsNullOrEmpty(Tags))
        //        return Tags.Split("#",StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        //    else
        //        return null;
        //}
        #endregion
    }
}
