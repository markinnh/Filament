
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

//using System.Text.Json.Serialization;
using System.Linq;
using MyLibraryStandard.Attributes;
using System.Windows.Input;

using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using DataDefinitions.Interfaces;
using LiteDB;
using DataDefinitions.LiteDBSupport;

namespace DataDefinitions.Models
{
    // TODO: Develop a UI for SpoolDefinition; Add, Update, Delete
    [UIHints(AddType = "Spool Definition")]
    public class SpoolDefn : ParentLinkedDataObject<VendorDefn>, ITrackUsable
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
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public override bool IsModified
        {
            get => base.IsModified || Inventory?.Count(inv => inv.IsModified) > 0;
            set => base.IsModified = value;
        }
        [JsonIgnore, BsonIgnore]
        public override bool IsValid => !double.IsNaN(SpoolDiameter) && !double.IsNaN(DrumDiameter)
            && !double.IsNaN(Weight) && !double.IsNaN(SpoolWidth) && Parent != null;

        //public override bool InDatabase => SpoolDefnId != default;

        private string description = "Black Plastic";
        [XmlAttribute("description")]
        public string Description
        {
            get => description;
            set => Set<string>(ref description, value);
        }


        private double spoolDiameter = Constants.DefaultSpoolDiameter;
        /// <summary>
        /// Gets or sets the spool diameter.
        /// </summary>
        /// <value>
        /// The spool diameter.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) }), XmlAttribute("spoolDia")]
        public double SpoolDiameter
        {
            get => spoolDiameter;
            set => Set<double>(ref spoolDiameter, value);
        }
        private double drumDiameter = Constants.DefaultDrumDiameter;
        /// <summary>
        /// Gets or sets the minimum diameter.
        /// </summary>
        /// <value>
        /// The minimum diameter.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) }), XmlAttribute("drumDia")]
        public double DrumDiameter
        {
            get => drumDiameter; set
            {
                if (Set<double>(ref drumDiameter, value))
                {
                    spoolDepth = (spoolDiameter - drumDiameter) / 2;
                    OnPropertyChanged(nameof(SpoolDepth));
                }
            }
        }

        private double spoolWidth = double.NaN;
        /// <summary>
        /// Gets or sets the width of the spool.
        /// </summary>
        /// <value>
        /// The width of the spool.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) }), XmlAttribute("spoolWidth")]
        public double SpoolWidth
        {
            get => spoolWidth;
            set => Set<double>(ref spoolWidth, value);
        }
        [JsonIgnore, BsonIgnore]
        public bool CanUseDepthMeasurement => !double.IsNaN(spoolDiameter) && !double.IsNaN(drumDiameter) && !double.IsNaN(spoolWidth);
        [JsonIgnore, BsonIgnore]
        public double MaxDepth => !double.IsNaN(spoolDiameter) && !double.IsNaN(drumDiameter) ? (spoolDiameter - drumDiameter) / 2 : double.NaN;

        private double spoolDepth = double.NaN;
        [JsonIgnore, XmlIgnore]
        public double SpoolDepth
        {
            get => spoolDepth;
            set
            {
                if (Set<double>(ref spoolDepth, value) && !double.IsNaN(spoolDepth) && !double.IsNaN(spoolDiameter))
                {
                    DrumDiameter = spoolDiameter - (2 * spoolDepth);
                }
            }
        }

        //private int filamentID;

        //public int FilamentID
        //{
        //    get => filamentID;
        //    set => Set<int>(ref filamentID, value);
        //}

        private int spoolDefnID;
        [XmlAttribute("ID"), JsonPropertyName("ID"), BsonIgnore]
        public int SpoolDefnId
        {
            get => spoolDefnID;
            set => Set<int>(ref spoolDefnID, value);
        }

        private bool stopUsing;
        /// <summary>
        /// Gets or sets a value indicating whether to stop using.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop using]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("stopUsing")]
        public bool StopUsing
        {
            get => stopUsing;
            set => Set<bool>(ref stopUsing, value);
        }


        //private FilamentDefn filament;
        ///// <summary>
        ///// Gets or sets the filament.
        ///// </summary>
        ///// <value>
        ///// The filament.
        ///// </value>

        //public FilamentDefn? Filament
        //{
        //    get => filament;
        //    set => Set<FilamentDefn>(ref filament, value);
        //}

        private double weight = Constants.StandardSpoolLoad;
        /// <summary>
        /// Gets or sets the weight.  In Kg, default is 1Kg.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) }), XmlAttribute("weight")]
        public double Weight
        {
            get => weight;
            set => Set<double>(ref weight, value);
        }

        private bool verified;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SpoolDefn"/> has it's dimensions verified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verified; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("verified")]
        public bool Verified
        {
            get => verified;
            set => Set<bool>(ref verified, value);
        }

        private int vendorID;
        /// <summary>
        /// Gets or sets the vendor identifier.
        /// </summary>
        /// <value>
        /// The vendor identifier.
        /// </value>
        [XmlAttribute("VendID"), BsonIgnore]
        public int VendorDefnId
        {
            get => vendorID;
            set => Set<int>(ref vendorID, value);
        }

        //public VendorDefn Vendor { get; set; }
        private VendorDefn vendor;
        /// <summary>
        /// Gets or sets the vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>
        [JsonIgnore, XmlIgnore, BsonRef]
        public VendorDefn Vendor
        {
            get => vendor;
            set
            {
                if (Set<VendorDefn>(ref vendor, value) && vendor != null && !InDataOperations)
                    vendorID = vendor.VendorDefnId;
            }
        }

        private bool showInUse;
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public bool ShowInUse
        {
            get => showInUse;
            set
            {
                Set<bool>(ref showInUse, value);
                OnPropertyChanged(nameof(FilteredInventory));
            }
        }

        public virtual ObservableCollection<InventorySpool> Inventory { get; set; }
        [JsonIgnore, BsonIgnore]
        public IEnumerable<InventorySpool> FilteredInventory => showInUse ? Inventory.Where(inv => !inv.StopUsing) : Inventory;
        /// <summary>
        /// Gets the name of the spool.
        /// </summary>
        /// <value>
        /// The name of the spool.
        /// </value>
        [JsonIgnore, BsonIgnore]
        public string SpoolName => $"{Vendor?.Name ?? Constants.DefaultVendorName} - {Weight}Kg";
        // TODO: Add Inventory Spools as a collection to spool defn

        public SpoolDefn()
        {
            Inventory = new ObservableCollection<InventorySpool>();
            InitEventHandlers();
            //Init();
        }
        private void InitEventHandlers()
        {
            Inventory.CollectionChanged += OInventory_CollectionChanged;
        }
        public void LinkToInventorySpools()
        {
            foreach (var inventory in Inventory)
                inventory.Subscribe(InventorySpool_PropertyChanged);
        }

        private void OInventory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Collection changed {e.Action}, count {e.NewItems?.Count}");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    if (item is InventorySpool inventorySpool)
                    {
                        inventorySpool.Parent = this;
                        //inventorySpool.SpoolDefn = this;
                        //inventorySpool.SpoolDefnId = spoolDefnID;
                        inventorySpool.Subscribe(WatchContainedHandler);
                        inventorySpool.WatchContained();
                    }
                if (!InDataOperations)
                    IsModified = true;
                //OnPropertyChanged(nameof(IsModified));
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is InventorySpool spool)
                    {

                    }
                }
                IsModified = true;
            }

            //throw new NotImplementedException();
        }
        public override void WatchContained()
        {
            foreach (var inventory in Inventory)
            {
                inventory.Subscribe(WatchContainedHandler);
                inventory.WatchContained();
            }
        }
        public override void UnWatchContained()
        {
            foreach (var inventory in Inventory)
            {
                inventory.Unsubscribe(WatchContainedHandler);
                inventory.UnWatchContained();
            }
        }
        public override string UIHintAddType() => typeof(InventorySpool).GetCustomAttribute<UIHintsAttribute>()?.AddType ?? string.Empty;
        private void InventorySpool_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Property Changed in {sender?.GetType().Name} for {e.PropertyName}");
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
            //throw new NotImplementedException();
        }

        internal SpoolDefn(double spoolDiameter, double minimumDiameter, double spoolWidth, int filamentID, int vendorID, bool verified = true)
        {
            //Init();
            SpoolDiameter = spoolDiameter;
            DrumDiameter = minimumDiameter;
            //FilamentDiameter = filamentDiameter;
            //FilamentID = filamentID;

            VendorDefnId = vendorID;

            SpoolWidth = spoolWidth;
            Verified = verified;
        }

        public static SpoolDefn CreateSpoolDefinition(double spoolDiameter, double minimumDiameter, double spoolWidth, int filamentID, int vendorID, bool verified = true)
        {
            var result = new SpoolDefn(spoolDiameter, minimumDiameter, spoolWidth, filamentID, vendorID, verified);
            System.Diagnostics.Debug.Assert(result != null);
            //result.EstablishLink(document);
            return result;
        }

        //internal override int KeyID
        //{
        //    get => spoolDefnID;
        //    set
        //    {
        //        bool assignChildren = !InDatabase;
        //        Set(ref spoolDefnID, value);
        //        foreach (var item in Inventory)
        //        {
        //            item.SpoolDefnId = spoolDefnID;
        //            if (assignChildren)
        //                item.AssignKey(Document.Counters.NextID(item));
        //        }
        //    }
        //}
        //public override void UpdateItem<TContext>() 
        //{
        //    if (IsValid)
        //    {
        //        using (TContext context = new TContext())
        //        {
        //            int expectedUpdate = 0;
        //            expectedUpdate += context.SetDataItemsState<InventorySpool>(Inventory.Where(inv => Added(inv)), Microsoft.EntityFrameworkCore.EntityState.Added);
        //            expectedUpdate += context.SetDataItemsState(Inventory.Where(inv => Modified(inv)), Microsoft.EntityFrameworkCore.EntityState.Modified);
        //            //expectedUpdate += context.SetDataItemsState(Inventory.Where(inv => inv.MarkedForDeletion && inv.InDatabase), Microsoft.EntityFrameworkCore.EntityState.Deleted);
        //            foreach (var inv in Inventory)
        //            {
        //                expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
        //                expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);
        //                //expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => dm.MarkedForDeletion && dm.InDatabase), Microsoft.EntityFrameworkCore.EntityState.Deleted);
        //            }

        //            try
        //            {
        //                if (InDatabase)
        //                    context.Update(this);
        //                else
        //                    context.Add(this);

        //                var updateCount = context.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine($"Unable to complete data operation, {ex.Message}");
        //            }
        //        }
        //    }
        //    //throw new NotImplementedException();
        //}
        /*
        internal override void UpdateContainedItemEntryState<TContext>(TContext context)
        {
            context.SetDataItemsState(Inventory.Where(inv => Modified(inv)), Microsoft.EntityFrameworkCore.EntityState.Modified);
            context.SetDataItemsState(Inventory.Where(inv => Added(inv)), Microsoft.EntityFrameworkCore.EntityState.Added);
            foreach (var item in Inventory)
            {
                foreach (DepthMeasurement depthMeasurement in item.DepthMeasurements)
                    depthMeasurement.UpdateContainedItemEntryState<TContext>(context);
            }
        }*/
        public override void SetContainedModifiedState(bool state)
        {
            IsModified = state;
            foreach (var inv in Inventory)
            {
                inv.SetContainedModifiedState(state);
                foreach (var dm in inv.DepthMeasurements)
                {
                    dm.IsModified = state;
                }
            }
        }
        internal override void LinkToParent(VendorDefn parent)
        {
            base.LinkToParent(parent);
            foreach (var inv in Inventory)
                inv.LinkToParent(this);
        }
        public override void PostDataRetrieveActions()
        {
            InitEventHandlers();
            WatchContained();
            foreach (var inv in Inventory)
                inv.PostDataRetrieveActions();
        }
        internal override void PrepareToSave(LiteDBDal dBDal)
        {
            foreach (var inv in Inventory)
                inv.PrepareToSave(dBDal);
        }
        //public override void LinkChildren<ParentType>(ParentType parent)
        //{
        //    if (parent is Models.VendorDefn defn)
        //    {
        //        Vendor = defn;
        //        foreach (var inv in Inventory)
        //            inv.LinkChildren<Models.SpoolDefn>(this);
        //    }
        //}
        //internal override void UpdateContainedItems()
        //{
        //    foreach (var inv in Inventory)
        //    {
        //        if (inv.IsValid && !inv.InDatabase)
        //            inv.AssignKey(Document.Counters.NextID(inv));

        //        inv.UpdateContainedItems();
        //    }
        //}
        /*        #region IEditableObject Implementation
                struct BackupData
                {
                    public string Description { get; set; }
                    public double Weight { get; set; }
                    public bool StopUsing { get; set; }
                    public double SpoolWidth { get; set; }
                    public double SpoolDiameter { get; set; }
                    public double DrumDiameter { get; set; }
                    public bool Verified { get; set; }

                    internal BackupData(string description, double weight, bool stopUsing, double spoolWidth, double spoolDiameter, double drumDiameter, bool verified)
                    {
                        Description = description;
                        Weight = weight;
                        StopUsing = stopUsing;
                        SpoolWidth = spoolWidth;
                        SpoolDiameter = spoolDiameter;
                        DrumDiameter = drumDiameter;
                        Verified = verified;
                    }
                }
                BackupData backupData;
                void IEditableObject.BeginEdit()
                {
                    if (!InEdit)
                    {
                        backupData = new BackupData(Description, Weight, StopUsing, SpoolWidth, SpoolDiameter, DrumDiameter, Verified);
                        InEdit = true;
                    }
                    //throw new NotImplementedException();
                }

                void IEditableObject.CancelEdit()
                {
                    if (InEdit)
                    {
                        Description = backupData.Description;
                        Weight = backupData.Weight;
                        StopUsing = backupData.StopUsing;
                        SpoolDiameter = backupData.SpoolDiameter;
                        SpoolWidth = backupData.SpoolWidth;
                        DrumDiameter = backupData.DrumDiameter;
                        Verified = backupData.Verified;
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
                #endregion*/
    }
}
