using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using Filament_Db.DataContext;
using MyLibraryStandard.Attributes;
using System.ComponentModel;

namespace Filament_Db.Models
{
    public class InventorySpool : DatabaseObject
    {
        public static event InDataOpsChangedHandler? InDataOpsChanged;

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
        public override bool InDataOperations => inDataOps;

        public override bool IsModified { get => base.IsModified || DepthMeasurements.Count(dm => dm.IsModified) > 0; set => base.IsModified = value; }

        public override bool IsValid => FilamentDefn!=null && !string.IsNullOrEmpty(ColorName) && SpoolDefn!=null;

        public override bool SupportsDelete => true;
        public override bool InDatabase => InventorySpoolId !=default; 
        public int InventorySpoolId { get; set; }

        public int FilamentDefnId { get; set; }

        private FilamentDefn? filamentDefn;
        [Affected(Names =new string[] {nameof(IsValid)})]
        public FilamentDefn? FilamentDefn
        {
            get => filamentDefn;
            set
            {
                if (Set(ref filamentDefn, value) && filamentDefn != null)
                    FilamentDefnId = filamentDefn.FilamentDefnId;
            }
        }

        public int SpoolDefnId { get; set; }
        private SpoolDefn? spoolDefn;

        public SpoolDefn? SpoolDefn
        {
            get => spoolDefn;
            set
            {
                if (Set<SpoolDefn?>(ref spoolDefn, value) && spoolDefn != null)
                    SpoolDefnId = spoolDefn.SpoolDefnID;
            }
        }

        private string? colorName;

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

        [NotMapped]
        public int AgeInDays => (DateTime.Today - DateOpened).Days;
        [NotMapped]
        public string Name => $"{ColorName} - {InventorySpoolId}";

        //private string? ignoreThis="Ignore This";
        //[NotMapped]
        //public string IgnoreThis
        //{
        //    get => ignoreThis="Ignore This";
        //    set => Set<string>(ref ignoreThis, value);
        //}

        public virtual ICollection<DepthMeasurement> DepthMeasurements { get; set; }
        [NotMapped]
        public IEnumerable<FilamentDefn>? Filaments => GetFilaments();
        
        protected IEnumerable<FilamentDefn>? GetFilaments()
        {
            if (Singleton<DataLayer>.Instance.GetFilteredSettings(se=> se.Name=="SelectShowFlag").SingleOrDefault() is Setting setting)
            {
                if (setting.Value == "ShowAll")
                    return Singleton<DataLayer>.Instance.FilamentList;
                else
                    return Singleton<DataLayer>.Instance.GetFilteredFilaments(fi => !fi.StopUsing);
            }
            else
                return Singleton<DataLayer>.Instance.FilamentList;
        }
        public InventorySpool()
        {
            DepthMeasurements = new ObservableCollection<DepthMeasurement>();
            if(DepthMeasurements is ObservableCollection<DepthMeasurement> Measurement)
                Measurement.CollectionChanged += Measurement_CollectionChanged;
            if (!InDataOpsChanged?.GetInvocationList().Contains(InventorySpool_InDataOpsChanged) ?? true)
                InDataOpsChanged += InventorySpool_InDataOpsChanged;
        }
        
        internal override void WatchContained()
        {
            foreach(var dm in DepthMeasurements)
                dm.Subscribe(WatchContainedHandler);
        }
        internal override void UnWatchContained()
        {
            foreach (var dm in DepthMeasurements)
                dm.Unsubscribe(WatchContainedHandler);
        }
        protected override void WatchContainedHandler(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));

        }
        private void Measurement_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    if (item is DepthMeasurement measurement)
                    {
                        measurement.InventorySpool = this;
                        measurement.Subscribe(WatchContainedHandler);
                    }
                OnPropertyChanged(nameof(IsModified));
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                IsModified = true;
            //throw new NotImplementedException();
        }

        private void HandleMeasurementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        ~InventorySpool()
        {
            if (InDataOpsChanged?.GetInvocationList().Contains(InventorySpool_InDataOpsChanged) ?? false)
                InDataOpsChanged -= InventorySpool_InDataOpsChanged;

            if(DepthMeasurements is ObservableCollection<DepthMeasurement> measurements)
                measurements.CollectionChanged -= Measurement_CollectionChanged;
            
            UnWatchContained();
        }
        private void InventorySpool_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }
        public override void UpdateItem()
        {
            if (IsValid)
            {
                using (FilamentContext context = new FilamentContext())
                {
                    int expectedUpdateCount = 0;

                    expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
                    expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where((dm) => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);

                    if (InDatabase)
                        context.Update(this);
                    else
                        context.Add(this);

                    var changeCount = context.SaveChanges();
                    
                }
                SetContainedModifiedState(false);
            }
        }
        public override void SetContainedModifiedState(bool state)
        {
            IsModified = state;
            foreach(var dm in DepthMeasurements)
                dm.IsModified = state;
        }

        public void Link(List<FilamentDefn> filaments)
        {
            if(FilamentDefnId!= default)
            {
                FilamentDefn=filaments.Single(fil=>fil.FilamentDefnId==FilamentDefnId);
            }
        }
    }
}
