using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using MyLibraryStandard.Attributes;
using MyLibraryStandard;
using System.Reflection;
using System.ComponentModel;

namespace Filament_Db.Models
{
    public enum DensityType { Defined = 0x8000, Measured }
    public class DensityAlias : DatabaseObject, IDensity, INotifyContainer
    {
        const int MinimumDensityMeasurementsRequired = 3;
        
        public static event InDataOpsChangedHandler? InDataOpsChanged;

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
        public override bool InDatabase => densityAliasId!=default;
        [NotMapped]
        public bool LinkedToCollectionChangedEH { get; set; } = false;
        [NotMapped]
        public bool IsLinkedToNotifyContainer => NotifyContainer != null;
        private int densityAliasId;

        public int DensityAliasId
        {
            get => densityAliasId;
            set => Set<int>(ref densityAliasId, value);
        }

        private DensityType densityType;
        [Affected(Names = new[] { nameof(Density) }),
            ContainerPropertiesAffected(new[]
            {
                nameof(Models.FilamentDefn.MgPerMM),
                nameof(Models.FilamentDefn.MeasuredDensityVisible),
                nameof(Models.FilamentDefn.DefinedDensityVisible)
            })]
        public DensityType DensityType
        {
            get => densityType;
            set => Set<DensityType>(ref densityType, value);
        }

        private double definedDensity;

        public event NotifyContainerHandler? NotifyContainer;

        public double DefinedDensity
        {
            get => definedDensity;
            set => Set<double>(ref definedDensity, value);
        }
        public int FilamentDefnId { get; set; }
        public FilamentDefn? FilamentDefn { get; set; }
        
        [NotMapped, ContainerPropertiesAffected(new[] { nameof(Models.FilamentDefn.MgPerMM) })]
        public double Density
        {
            get => densityType == DensityType.Defined ? definedDensity :
                MeasuredDensity?.Count(m => !double.IsNaN(m.Density)) >= MinimumDensityMeasurementsRequired ? MeasuredDensity.Average(m => m.Density) :
                double.NaN;
        }

        public DensityAlias()
        {
            if (MeasuredDensity is ObservableCollection<MeasuredDensity> col && !LinkedToCollectionChangedEH)
            {
                col.CollectionChanged += Col_CollectionChanged;
                LinkedToCollectionChangedEH = true;
            }
        }
        ~DensityAlias()
        {
            if (MeasuredDensity is ObservableCollection<MeasuredDensity> col && LinkedToCollectionChangedEH)
                col.CollectionChanged -= Col_CollectionChanged;
            if (NotifyContainer?.GetInvocationList().Where(d => d is NotifyContainerHandler).Cast<NotifyContainerHandler>() is IEnumerable<NotifyContainerHandler> handlers)
            {
                System.Diagnostics.Debug.WriteLine($"Removing event handlers in {nameof(DensityAlias)}");
                foreach (var handler in handlers)
                    NotifyContainer -= handler;
            }
            UnWatchContained();
        }
        internal override void WatchContained()
        {
            foreach(var item in MeasuredDensity)
                item.Subscribe(WatchContainedHandler);
        }
        internal override void UnWatchContained()
        {
            foreach (var item in MeasuredDensity)
                item.Unsubscribe(WatchContainedHandler);
        }
        protected override void WatchContainedHandler(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
        }
        private void Col_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Density));

            if (GetType().GetProperty(nameof(Density))?.GetCustomAttribute<ContainerPropertiesAffectedAttribute>() is ContainerPropertiesAffectedAttribute attrib)
            {
                DoNotify(new NotifyContainerEventArgs(attrib.Names));
            }
            else
                System.Diagnostics.Debug.Assert(false, "Unable to update the container after the collection changed");
            //DoNotify(new NotifyContainerEventArgs(new string[] { nameof(Models.FilamentDefn.MgPerMM) }));
        }

        public void DoNotify(NotifyContainerEventArgs args) => NotifyContainer?.Invoke(this, args);

        internal void Subscribe(NotifyContainerHandler handler)
        {
            if (handler != null)
            {
                if (!NotifyContainer?.GetInvocationList().Contains(handler) ?? true)
                    NotifyContainer += handler;
            }
        }
        internal void UnSubscribe(NotifyContainerHandler handler)
        {
            if (handler != null)
            {
                if (NotifyContainer?.GetInvocationList().Contains(handler) ?? false)
                    NotifyContainer -= handler;
            }
        }
        public ICollection<MeasuredDensity> MeasuredDensity { get; set; } = new ObservableCollection<MeasuredDensity>();
    }
}
