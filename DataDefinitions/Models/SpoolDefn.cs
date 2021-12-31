
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
using System.Linq;
using MyLibraryStandard.Attributes;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace DataDefinitions.Models
{
    // TODO: Develop a UI for SpoolDefinition; Add, Update, Delete
    public class SpoolDefn : DatabaseObject
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

        public override bool IsModified { get => base.IsModified || Inventory?.Count(inv => inv.IsModified) > 0; set => base.IsModified = value; }

        public override bool IsValid => spoolDiameter != double.NaN && drumDiameter != double.NaN
            && weight != double.NaN && spoolWidth != double.NaN && Vendor != null;

        public override bool InDatabase => SpoolDefnId != default;

        private string description="Black Plastic";
        [MaxLength(128)]
        public string Description
        {
            get => description;
            set => Set<string>(ref description, value);
        }


        private double spoolDiameter = double.NaN;
        /// <summary>
        /// Gets or sets the spool diameter.
        /// </summary>
        /// <value>
        /// The spool diameter.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) })]
        public double SpoolDiameter { get => spoolDiameter; set => Set<double>(ref spoolDiameter, value); }
        private double drumDiameter = double.NaN;
        /// <summary>
        /// Gets or sets the minimum diameter.
        /// </summary>
        /// <value>
        /// The minimum diameter.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) })]
        public double DrumDiameter { get => drumDiameter; set => Set<double>(ref drumDiameter, value); }

        private double spoolWidth = double.NaN;
        /// <summary>
        /// Gets or sets the width of the spool.
        /// </summary>
        /// <value>
        /// The width of the spool.
        /// </value>
        [Affected(Names = new string[] { nameof(IsValid) })]
        public double SpoolWidth
        {
            get => spoolWidth;
            set => Set<double>(ref spoolWidth, value);
        }
        [NotMapped]
        public bool CanUseDepthMeasurement => !double.IsNaN(spoolDiameter) && !double.IsNaN(drumDiameter) && !double.IsNaN(spoolWidth);

        //private int filamentID;

        //public int FilamentID
        //{
        //    get => filamentID;
        //    set => Set<int>(ref filamentID, value);
        //}

        private int spoolDefnID;
        
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
        [Affected(Names = new string[] { nameof(IsValid) })]
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
        [NotMapped]
        public bool ShowInUse
        {
            get => showInUse;
            set
            {
                Set<bool>(ref showInUse, value);
                OnPropertyChanged(nameof(FilteredInventory));
            }
        }

        public virtual ICollection<InventorySpool> Inventory { get; set; }
        [NotMapped]
        public IEnumerable<InventorySpool> FilteredInventory => showInUse ? Inventory.Where(inv => !inv.StopUsing) : Inventory;
        /// <summary>
        /// Gets the name of the spool.
        /// </summary>
        /// <value>
        /// The name of the spool.
        /// </value>
        [NotMapped]
        public string SpoolName => $"{Vendor?.Name ?? Constants.DefaultVendorName} - {Weight}Kg";
        // TODO: Add Inventory Spools as a collection to spool defn

        public SpoolDefn()
        {
            Inventory = new ObservableCollection<InventorySpool>();
            if (Inventory is ObservableCollection<InventorySpool> oInventory)
                oInventory.CollectionChanged += OInventory_CollectionChanged;

            //Init();
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
                        inventorySpool.SpoolDefn = this;
                        inventorySpool.SpoolDefnId = spoolDefnID;
                        inventorySpool.Subscribe(InventorySpool_PropertyChanged);
                    }
                OnPropertyChanged(nameof(IsModified));
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is InventorySpool spool)
                    {
                        //FilamentContext.DeleteItems(e.OldItems);
                    }
                }
                IsModified = true;
            }

            //throw new NotImplementedException();
        }

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
            //Filament = DataContext.FilamentContext.GetFilament(fil=>fil.FilamentDefnId == filamentID);
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

        public override void UpdateItem<TContext>() 
        {
            if (IsValid)
            {
                using (TContext context = new TContext())
                {
                    int expectedUpdate = 0;
                    expectedUpdate += context.SetDataItemsState<InventorySpool>(Inventory.Where(inv => Added(inv)), Microsoft.EntityFrameworkCore.EntityState.Added);
                    expectedUpdate += context.SetDataItemsState(Inventory.Where(inv => Modified(inv)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                    //expectedUpdate += context.SetDataItemsState(Inventory.Where(inv => inv.MarkedForDeletion && inv.InDatabase), Microsoft.EntityFrameworkCore.EntityState.Deleted);
                    foreach (var inv in Inventory)
                    {
                        expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
                        expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);
                        //expectedUpdate += context.SetDataItemsState(inv.DepthMeasurements.Where(dm => dm.MarkedForDeletion && dm.InDatabase), Microsoft.EntityFrameworkCore.EntityState.Deleted);
                    }

                    try
                    {
                        if (InDatabase)
                            context.Update(this);
                        else
                            context.Add(this);

                        var updateCount = context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Unable to complete data operation, {ex.Message}");
                    }
                }
            }
            //throw new NotImplementedException();
        }
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
    }
}
