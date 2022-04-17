using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
/*
 * 
 */
namespace DataDefinitions.Models
{
    /// <summary>
    /// Settings specific to a certain type of filament
    /// </summary>
    public class VendorSettingsConfig : DatabaseObject
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
        [Affected(Names =new string[] {nameof(InDatabase)})]
        public override bool IsModified { get => base.IsModified || ConfigItems.Count(ci => ci.IsModified) > 0; set => base.IsModified = value; }
        public int VendorSettingsConfigId { get; set; }
        public int VendorDefnId { get; set; }
        public virtual VendorDefn VendorDefn { get; set; }

        private int filamentDefnId;

        public int FilamentDefnId
        {
            get => filamentDefnId;
            set => Set<int>(ref filamentDefnId, value);
        }

        private FilamentDefn filamentDefn;

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

        public override bool InDatabase => VendorSettingsConfigId != default;
        public override bool IsValid => FilamentDefnId != default;
        public ICollection<ConfigItem> ConfigItems { get; set; }

        public VendorSettingsConfig()
        {
            if (new ObservableCollection<ConfigItem>() is ObservableCollection<ConfigItem> configItems)
            {
                ConfigItems = configItems;
                configItems.CollectionChanged += ConfigItems_CollectionChanged;
            }

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

        internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        {
            if (InDatabase)
            {
                context.SetDataItemsState(ConfigItems.Where(ci => Modified(ci)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                //context.SetDataItemsState(ConfigItems.Where(ci => Added(ci)), Microsoft.EntityFrameworkCore.EntityState.Added);
            }

            context.SetDataItemsState(ConfigItems.Where(ci => Added(ci)), Microsoft.EntityFrameworkCore.EntityState.Added);
        }

        public override void SetContainedModifiedState(bool state)
        {
            Trace.Indent();
            
            foreach (var item in ConfigItems)
            {
                Trace.WriteLine($"item has key {item.ConfigItemId}, {{{item.PrintSettingDefn?.Definition}={item.TextValue}}}");
                item.IsModified = state;
            }
            IsModified = state;
            
            Trace.Unindent();
        }
    }
}
