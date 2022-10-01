using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

using MyLibraryStandard.Attributes;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Reflection;

namespace DataDefinitions.Models
{
    [UIHints(AddType = "Inventory")]
    public class InventorySpool : DataDefinitions.DatabaseObject, IEditableObject
    {
        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the InventorySpool objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        [JsonIgnore]
        public override bool InDataOperations => inDataOps;
        [JsonIgnore]
        public override bool IsModified { get => base.IsModified || DepthMeasurements.Count(dm => dm.IsModified) > 0; set => base.IsModified = value; }
        [JsonIgnore]
        public override bool IsValid => FilamentDefn != null && !string.IsNullOrEmpty(ColorName) && SpoolDefn != null && SpoolDefnId != default;
        [JsonIgnore]
        public override bool SupportsDelete => true;
        [JsonIgnore]
        public override bool InDatabase => InventorySpoolId != default;
        public int InventorySpoolId { get; set; }

        public int FilamentDefnId { get; set; }

        private FilamentDefn filamentDefn;
        [Affected(Names = new string[] { nameof(IsValid) }), JsonIgnore]
        public FilamentDefn FilamentDefn
        {
            get => filamentDefn;
            set
            {
                if (Set(ref filamentDefn, value) && filamentDefn != null)
                    FilamentDefnId = filamentDefn.FilamentDefnId;
            }
        }

        public int SpoolDefnId { get; set; }
        private SpoolDefn spoolDefn;
        [JsonIgnore]
        public SpoolDefn SpoolDefn
        {
            get => spoolDefn;
            set
            {
                if (Set<SpoolDefn>(ref spoolDefn, value) && spoolDefn != null)
                    SpoolDefnId = spoolDefn.SpoolDefnId;
            }
        }

        private string colorName;

        public string ColorName
        {
#pragma warning disable CS8603 // Possible null reference return.
            get => colorName;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
            set => Set<string>(ref colorName, value);
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private DateTime dateOpened = DateTime.Today;

        public DateTime DateOpened
        {
            get => dateOpened;
            set => Set(ref dateOpened, value);
        }
        private bool stopUsing;

        public bool StopUsing
        {
            get => stopUsing;
            set => Set<bool>(ref stopUsing, value);
        }

        [NotMapped, JsonIgnore]
        public int AgeInDays => (DateTime.Today - DateOpened).Days;
        [NotMapped, JsonIgnore]
        public string Name => $"{ColorName} - {InventorySpoolId}";
        [NotMapped, JsonIgnore]
        public string VendorName => SpoolDefn?.Vendor?.Name ?? "Undefined";
        [NotMapped, JsonIgnore]
        public string SpoolDescription => SpoolDefn?.Description ?? "Undefined";
        public double CalcInitialDepth()
        {
            const double utilizationFactor = 0.95;
            const double layerOverlap = .95;
            bool initialLayer = true;
            if (SpoolDefn != null && FilamentDefn != null)
            {
                var initialLength = InitialLength;
                if (!double.IsNaN(initialLength))
                {
                    double depth = (SpoolDefn.SpoolDiameter - SpoolDefn.DrumDiameter) / 2 + (FilamentDefn.Diameter / 2);
                    double length = 0;
                    do
                    {
                        var turns = SpoolDefn.SpoolWidth / FilamentDefn.Diameter * utilizationFactor;
                        var windLength = (SpoolDefn.SpoolDiameter - depth * 2) * Math.PI / 1000;
                        var amountAdded = turns * windLength;
                        length += amountAdded;
                        depth -= FilamentDefn.Diameter * (initialLayer ? 1.0 : layerOverlap);
                        initialLayer = false;
                    } while (depth > 0 && length < initialLength);
                    return depth;
                }
                return double.NaN;
            }
            else
                return double.NaN;
        }
        [NotMapped]
        public double InitialLength
        {
            get
            {
                if (SpoolDefn != null && FilamentDefn != null)
                {
                    return FilamentMath.LengthFromWeightBasedOnDensity(FilamentDefn.DensityAlias, SpoolDefn.Weight * 1000, FilamentDefn.Diameter) / 1000;

                }
                else
                    return double.NaN;
            }
        }
        //private string? ignoreThis="Ignore This";
        //[NotMapped]
        //public string IgnoreThis
        //{
        //    get => ignoreThis="Ignore This";
        //    set => Set<string>(ref ignoreThis, value);
        //}

        public virtual ICollection<DepthMeasurement> DepthMeasurements { get; set; }
        // if this is used in the wpf app it will need to exist in some form, probably for the UWP app,
        // BindableInventorySpool will have to be inherited from InventorySpool
        //[NotMapped]
        //public IEnumerable<FilamentDefn> Filaments => GetFilaments();

        //protected IEnumerable<FilamentDefn> GetFilaments()
        //{
        //    if (Singleton<DataLayer>.Instance.GetFilteredSettings(se => se.Name == "SelectShowFlag").SingleOrDefault() is Setting setting)
        //    {
        //        if (setting.Value == "ShowAll")
        //            return Singleton<DataLayer>.Instance.FilamentList;
        //        else
        //            return Singleton<DataLayer>.Instance.GetFilteredFilaments(fi => !fi.StopUsing);
        //    }
        //    else
        //        return Singleton<DataLayer>.Instance.FilamentList;
        //}
        public InventorySpool()
        {
            DepthMeasurements = new ObservableCollection<DepthMeasurement>();
            if (DepthMeasurements is ObservableCollection<DepthMeasurement> Measurement)
                Measurement.CollectionChanged += Measurement_CollectionChanged;

            InDataOpsChanged += InventorySpool_InDataOpsChanged;
        }

        public override void WatchContained()
        {
            foreach (var dm in DepthMeasurements)
                dm.Subscribe(WatchContainedHandler);
        }
        public override void UnWatchContained()
        {
            foreach (var dm in DepthMeasurements)
                dm.Unsubscribe(WatchContainedHandler);
        }
        public override string UIHintAddType() => typeof(DepthMeasurement).GetCustomAttribute<UIHintsAttribute>()?.AddType ?? string.Empty;
        private void Measurement_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    if (item is DepthMeasurement measurement)
                    {
                        measurement.InventorySpool = this;
                        if (!measurement.InDatabase)
                        {
                            measurement.InventorySpoolId = InventorySpoolId;
                            measurement.Depth1 = CalcInitialDepth();
                            measurement.Depth2 = CalcInitialDepth();
                        }
                        measurement.Subscribe(WatchContainedHandler);
                    }
                OnPropertyChanged(nameof(IsModified));
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                IsModified = true;
            //throw new NotImplementedException();
        }

        private void HandleMeasurementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        ~InventorySpool()
        {

            InDataOpsChanged -= InventorySpool_InDataOpsChanged;

            if (DepthMeasurements is ObservableCollection<DepthMeasurement> measurements)
                measurements.CollectionChanged -= Measurement_CollectionChanged;

            UnWatchContained();
        }
        private void InventorySpool_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }
        //public override void UpdateItem<TContext>()
        //{
        //    if (IsValid)
        //    {
        //        using (TContext context = new TContext())
        //        {
        //            int expectedUpdateCount = 0;

        //            expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
        //            expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where((dm) => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);

        //            if (InDatabase)
        //                context.Update(this);
        //            else
        //                context.Add(this);

        //            var changeCount = context.SaveChanges();

        //        }
        //        SetContainedModifiedState(false);
        //    }
        //}
        internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        {
            context.SetDataItemsState(DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
            context.SetDataItemsState(DepthMeasurements.Where(dm => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);
        }
        public override void SetContainedModifiedState(bool state)
        {

            foreach (var dm in DepthMeasurements)
                dm.IsModified = state;
            IsModified = state;
        }

        public void Link(IEnumerable<FilamentDefn> filaments)
        {
            if (FilamentDefnId != default)
            {
                FilamentDefn = filaments.Single(fil => fil.FilamentDefnId == FilamentDefnId);
            }
        }
        #region IEditableObject Implementation
        struct BackupData
        {
            public string ColorName { get; set; }
            public DateTime DateOpened { get; set; }
            public int FilamentDefnId { get; set; }
            public FilamentDefn FilamentDefn { get; set; }
            internal BackupData(InventorySpool inventorySpool)
            {
                ColorName = inventorySpool.ColorName;
                DateOpened = inventorySpool.DateOpened;
                FilamentDefnId = inventorySpool.FilamentDefnId;
                FilamentDefn = inventorySpool.FilamentDefn;
            }
        }
        BackupData backupData;
        void IEditableObject.BeginEdit()
        {
            if (!InEdit)
            {
                backupData = new BackupData(this);
                InEdit = true;
            }
            //throw new NotImplementedException();
        }

        void IEditableObject.CancelEdit()
        {
            if (InEdit)
            {
                ColorName = backupData.ColorName;
                DateOpened = backupData.DateOpened;
                FilamentDefnId = backupData.FilamentDefnId;
                FilamentDefn = backupData.FilamentDefn;
                SetContainedModifiedState(false);
                backupData = default(BackupData);
                InEdit = false;
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
