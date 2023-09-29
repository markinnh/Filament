using DataDefinitions.Interfaces;
using LiteDB;
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
/*
* 
*/
namespace DataDefinitions.Models
{
    /// <summary>
    /// Settings specific to a certain type of filament
    /// </summary>
    public class VendorPrintSettingsConfig : ParentLinkedDataObject<VendorDefn>
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
        [JsonIgnore]
        public override bool InDataOperations => InDataOps;
        //private bool isModified;
        [JsonIgnore, BsonIgnore]
        public override bool IsModified { get => base.IsModified || ConfigItems.Any(ci => ci.IsModified); set => base.IsModified = value; }
        //[JsonPropertyName("ID"),BsonIgnore]
        //public int VendorSettingsConfigId { get; set; }
        [JsonPropertyName("VendorID")]
        public int VendorDefnId { get; set; }
        [JsonIgnore, BsonIgnore]
        public virtual VendorDefn VendorDefn { get; set; }

        private int filamentDefnId;
        [JsonPropertyName("FilamentID")]
        public int FilamentDefnId
        {
            get => filamentDefnId;
            set => Set<int>(ref filamentDefnId, value);
        }
        //internal override int KeyID { get => VendorSettingsConfigId; set => VendorSettingsConfigId = value; }
        private FilamentDefn filamentDefn;
        [JsonIgnore, BsonIgnore]
        public virtual FilamentDefn FilamentDefn
        {
            get => filamentDefn;
            set
            {
                if (Set<FilamentDefn>(ref filamentDefn, value) && filamentDefn != null)
                    filamentDefnId = filamentDefn.FilamentDefnId;
            }
        }
        //private string colorName;
        //[MaxLength(128)]
        //public string ColorName
        //{
        //    get => colorName;
        //    set => Set<string>(ref colorName, value);
        //}
        //[JsonIgnore]
        //public override bool InDatabase => VendorSettingsConfigId != default;
        [JsonIgnore, BsonIgnore]
        public override bool IsValid => FilamentDefnId != default;
        public ObservableCollection<ConfigItem> ConfigItems { get; set; }

        public VendorPrintSettingsConfig()
        {
            ConfigItems = new ObservableCollection<ConfigItem>();
            InitEventHandlers();

        }
        protected void InitEventHandlers()
        {
            ConfigItems.CollectionChanged += ConfigItems_CollectionChanged;
        }

        public void LinkPrintSettings(IEnumerable<PrintSettingDefn> printSettingDefns)
        {
            foreach (ConfigItem item in ConfigItems)
                if (item.PrintSettingDefn == null && item.PrintSettingDefnId != default)
                    item.PrintSettingDefn = printSettingDefns.FirstOrDefault(ps => ps.PrintSettingDefnId == item.PrintSettingDefnId);
        }
        private void ConfigItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
            {
                foreach (ConfigItem item in e.NewItems)
                {
                    item.Subscribe(WatchContainedHandler);
                    //.EstablishLink(Document);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems.Count > 0)
            {
                foreach (ConfigItem item in e.OldItems)
                {
                    item.Unsubscribe(WatchContainedHandler);
                }
            }
            //throw new NotImplementedException();
        }
        //protected override void AssignKey(int myId)
        //{
        //    if (VendorSettingsConfigId == default)
        //        VendorSettingsConfigId = myId;
        //    else
        //        ReportKeyAlreadyInitialized();
        //}

        //public override void EstablishLink(IJsonFilamentDocument document)
        //{
        //    base.EstablishLink(document);
        //    InitEventHandlers();
        //    //foreach (var item in ConfigItems)
        //    //    item.EstablishLink(document);

        //}
        //internal override void UpdateContainedItems()
        //{
        //    foreach(var cfgItem in ConfigItems)
        //    {
        //        cfgItem.VendorSettingsConfigId = VendorSettingsConfigId;
        //        if (!cfgItem.InDatabase && cfgItem.IsValid)
        //            cfgItem.AssignKey(Document.Counters.NextID(cfgItem));
        //        cfgItem.UpdateContainedItems();
        //    }
        //}
        public override void WatchContained()
        {
            foreach (var item in ConfigItems)
                item.Subscribe(WatchContainedHandler);
        }
        public override void UnWatchContained()
        {
            foreach (var item in ConfigItems)
                item.Unsubscribe(WatchContainedHandler);
        }
        public void Link(IEnumerable<FilamentDefn> filamentDefns)
        {
            if (FilamentDefnId != default)
                FilamentDefn = filamentDefns.Single(fd => fd.FilamentDefnId == FilamentDefnId);
        }
        public void Link(IEnumerable<PrintSettingDefn> settingDefns)
        {
            foreach (var item in ConfigItems)
                if (item.PrintSettingDefnId != default)
                    item.PrintSettingDefn = settingDefns.First(sd => sd.PrintSettingDefnId == item.PrintSettingDefnId);
        }
        public override void PostDataRetrieveActions()
        {
            InitEventHandlers();
        }
        /*
        internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        {
            if (InDatabase)
            {
                context.SetDataItemsState(ConfigItems.Where(ci => Modified(ci)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                //context.SetDataItemsState(ConfigItems.Where(ci => Added(ci)), Microsoft.EntityFrameworkCore.EntityState.Added);
            }

            context.SetDataItemsState(ConfigItems.Where(ci => Added(ci)), Microsoft.EntityFrameworkCore.EntityState.Added);
        }
        */
        public override void SetContainedModifiedState(bool state)
        {
            Debug.Indent();

            foreach (var item in ConfigItems)
            {
                Debug.WriteLine($"item {{{item.PrintSettingDefn?.Definition}={item.TextValue}}}");
                item.IsModified = state;
            }
            IsModified = state;

            Debug.Unindent();
        }
    }
}
