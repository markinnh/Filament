using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
//using System.ComponentModel.DataAnnotations.Schema;
using MyLibraryStandard.Attributes;
using MyLibraryStandard;
using System.Reflection;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using LiteDB;
using DataDefinitions.Interfaces;

namespace DataDefinitions.Models
{
    /// <summary>
    /// Type of density measurement to be stored
    /// </summary>
    public enum DensityType { Defined = 0x8000, Measured }
    /// <summary>
    /// Allows storing either 'Defined' or 'Measured' densities
    /// </summary>

    public class DensityAlias : Observable,ITrackModified, IDensity, INotifyContainer
    {
        const int MinimumDensityMeasurementsRequired = 3;

        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        [JsonIgnore, BsonIgnore]
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
        [JsonIgnore, BsonIgnore]
        public bool InDataOperations => InDataOps;
        //[JsonIgnore]
        //public override bool InDatabase => densityAliasId != default;
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public bool LinkedToCollectionChangedEH { get; set; } = false;
        [JsonIgnore, BsonIgnore]
        public bool IsLinkedToNotifyContainer => NotifyContainer != null;
        private int densityAliasId;

        /// <summary>
        /// Database identifier
        /// </summary>
        [XmlAttribute("ID"), JsonPropertyName("ID"), BsonIgnore]
        public int DensityAliasId
        {
            get => densityAliasId;
            set => Set<int>(ref densityAliasId, value);
        }
        //internal override int KeyID
        //{
        //    get => densityAliasId;
        //    set
        //    {
        //        Set(ref densityAliasId, value);
        //        foreach (var item in MeasuredDensity)
        //            item.UpdateItem();

        //    }
        //}
        [BsonIgnore]
        public bool IsModified { get;  set; }
        private DensityType densityType;
        /// <summary>
        /// Type of density definition used
        /// </summary>
        /// <remarks>either defined or measured, normally, a defined density is preferred to a measured density.</remarks>
        /// <value>DensityType is either <b>Defined</b> or <b>Measured</b></value>
        [Affected(Names = new[] { nameof(Density) }),
                    ContainerPropertiesAffected(new[]
                    {
                nameof(Models.FilamentDefn.MgPerMM),
                nameof(Models.FilamentDefn.MeasuredDensityVisible),
                nameof(Models.FilamentDefn.DefinedDensityVisible)
                    }), XmlAttribute("densityType"),BsonField("type")]
        public DensityType DensityType
        {
            get => densityType;
            set => Set<DensityType>(ref densityType, value);
        }

        private double definedDensity;

        public event NotifyContainerHandler NotifyContainer;

        /// <summary>
        /// A defined density for a 'FilamentDefn'
        /// </summary>
        [XmlAttribute("definedDensity"), JsonPropertyName("Defined")]
        public double DefinedDensity
        {
            get => definedDensity;
            set => Set<double>(ref definedDensity, value);
        }
        /// <summary>
        /// Id of assigned filament
        /// </summary>
        /// <remarks>value must be set to store the object in the database.</remarks>
        /// <value>Valid FilamentDefnId</value>
        [XmlAttribute("filamentID"),BsonIgnore]
        public int FilamentDefnId { get; set; }
        /// <summary>
        /// reference to the applicable FilamentDefn
        /// </summary>
        /// <remarks>Stored in a separate table and relinked after retrieving from the database.</remarks>
        /// <value>FilamentDefn or null</value>
        [JsonIgnore, XmlIgnore,BsonIgnore]
        public FilamentDefn FilamentDefn { get; set; }

        /// <summary>
        /// An alias for either the 'DefinedDensity' or the 'MeasuredDensity'
        /// </summary>
        [JsonIgnore,BsonIgnore, ContainerPropertiesAffected(new[] { nameof(Models.FilamentDefn.MgPerMM) })]
        public double Density
        {
            get => densityType == DensityType.Defined ? definedDensity :
                MeasuredDensity?.Count(m => !double.IsNaN(m.Density)) >= MinimumDensityMeasurementsRequired ? MeasuredDensity.Average(m => m.Density) :
                double.NaN;
        }

        /// <summary>
        /// creates a default density alias, normally, just sets the DensityType member
        /// </summary>
        public DensityAlias()
        {
            if (MeasuredDensity is ObservableCollection<MeasuredDensity> col && !LinkedToCollectionChangedEH)
            {
                InitEventHandlers(col);
            }
        }

        private void InitEventHandlers(ObservableCollection<MeasuredDensity> col)
        {
            col.CollectionChanged += Col_CollectionChanged;
            LinkedToCollectionChangedEH = true;
        }

        /// <summary>
        /// releases eventhandlers for strong references
        /// </summary>
        /// <remarks>pertains mainly to original event type, although most event handlers are 'unwound' on object release</remarks>
        ~DensityAlias()
        {
            if (MeasuredDensity is ObservableCollection<MeasuredDensity> col && LinkedToCollectionChangedEH)
                col.CollectionChanged -= Col_CollectionChanged;
            if (NotifyContainer?.GetInvocationList().Where(d => d is NotifyContainerHandler).Cast<NotifyContainerHandler>() is IEnumerable<NotifyContainerHandler> handlers)
            {
                System.Diagnostics.Debug.WriteLine($"Removing event handlers in {nameof(DensityAlias)}");
                foreach (var handler in handlers)
                    if (handler != null)
                        NotifyContainer -= handler;
            }
            UnWatchContained();
        }
        //public override void EstablishLink(IJsonFilamentDocument document)
        //{
        //    base.EstablishLink(document);
        //    InitEventHandlers(MeasuredDensity);
        //    foreach (var item in MeasuredDensity)
        //        item.EstablishLink(document);
        //}
        public void LinkChildren<ParentType>(ParentType parent)
        {
            if (parent is FilamentDefn defn)
                FilamentDefn = defn;

        }
        //protected override void AssignKey(int myId)
        //{
        //    if (DensityAliasId == default)
        //    {
        //        DensityAliasId = myId;
        //    }
        //    else
        //        throw new ArgumentException(keyInitializedError);
        //}
        /// <summary>
        /// A debug only routine to allow testing the UI
        /// </summary>
        public void Prepopulate()
        {
#if DEBUG
            if (DensityType == DensityType.Measured)
            {
                MeasuredDensity.Clear();
                Random random = new Random();
                const int minRandom = 990;
                const int maxRandom = 1030;
                MeasuredDensity?.Add(new MeasuredDensity(2.98, random.Next(minRandom, maxRandom)));
                MeasuredDensity?.Add(new MeasuredDensity(2.99, random.Next(minRandom, maxRandom)));
                MeasuredDensity?.Add(new MeasuredDensity(3, random.Next(minRandom, maxRandom)));
                OnPropertyChanged(nameof(Density));
            }
#else
#endif
        }
        /// <summary>
        /// Add handler for the WatchContainedHandler
        /// </summary>
        public override void WatchContained()
        {
            foreach (var item in MeasuredDensity)
                item.Subscribe(WatchContainedHandler);
        }
        /// <summary>
        /// Remove handlers for the WatchContainedHandler
        /// </summary>
        public override void UnWatchContained()
        {
            foreach (var item in MeasuredDensity)
                item.Unsubscribe(WatchContainedHandler);
        }
        /// <summary>
        /// allows monitoring contained object properties
        /// </summary>
        /// <remarks>Probably will replace INotifyContainer events</remarks>
        protected override void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
        }
        private void Col_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

        /// <summary>
        /// routine to notify the container on a property change
        /// </summary>
        /// <remarks>can be called from base objects through the INotifyContainer interface</remarks>
        public void DoNotify(NotifyContainerEventArgs args) => NotifyContainer?.Invoke(this, args);

        /// <summary>
        /// Subscribe for the NotifyContainer events
        /// </summary>
        /// <remarks>Probably can be rolled into the WatchContainedHandler method where necessary</remarks>
        internal void Subscribe(NotifyContainerHandler handler)
        {
            if (handler != null)
            {
                if (!NotifyContainer?.GetInvocationList().Contains(handler) ?? true)
                    NotifyContainer += handler;
            }
        }
        /// <summary>
        /// Unsubscribe from the NotifyContainer event
        /// </summary>
        internal void UnSubscribe(NotifyContainerHandler handler)
        {
            if (handler != null)
            {
                if (NotifyContainer?.GetInvocationList().Contains(handler) ?? false)
                    NotifyContainer -= handler;
            }
        }
        //internal override void UpdateContainedItems()
        //{
        //    foreach (var measure in MeasuredDensity)
        //    {
        //        if (!measure.InDatabase && measure.IsValid)
        //            measure.AssignKey(Document.Counters.NextID(measure));
        //    }
        //}
        /// <summary>
        /// Collection of measurements
        /// </summary>
        /// <remarks>Applies when the DensityType is 'Measured'</remarks>
        [JsonPropertyName("Measured")]
        public ObservableCollection<MeasuredDensity> MeasuredDensity { get; set; } = new ObservableCollection<MeasuredDensity>();
    }
}
