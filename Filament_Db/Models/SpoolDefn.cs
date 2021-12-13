using Filament_Db.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
using System.Linq;


namespace Filament_Db.Models
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
        private double spoolDiameter;
        /// <summary>
        /// Gets or sets the spool diameter.
        /// </summary>
        /// <value>
        /// The spool diameter.
        /// </value>
        public double SpoolDiameter { get => spoolDiameter; set => Set<double>(ref spoolDiameter, value); }
        private double drumDiameter;
        /// <summary>
        /// Gets or sets the minimum diameter.
        /// </summary>
        /// <value>
        /// The minimum diameter.
        /// </value>
        public double DrumDiameter { get => drumDiameter; set => Set<double>(ref drumDiameter, value); }

        private double spoolWidth;
        /// <summary>
        /// Gets or sets the width of the spool.
        /// </summary>
        /// <value>
        /// The width of the spool.
        /// </value>
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

        public int SpoolDefnID
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
        private VendorDefn? vendor;
        /// <summary>
        /// Gets or sets the vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>

        public VendorDefn Vendor
        {
            get => vendor;
            set => Set<VendorDefn>(ref vendor, value);
        }

        public virtual IEnumerable<InventorySpool> Inventory { get; set; }
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
        [NotMapped]
        public bool InventoryNull => Inventory is null;


        private void OInventory_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Collection changed {e.Action}, count {e.NewItems?.Count}");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
                foreach (var item in e.NewItems)
                    if (item is InventorySpool inventorySpool)
                    {
                        inventorySpool.SpoolDefn = this;
                        inventorySpool.SpoolDefnId = spoolDefnID;
                        inventorySpool.PropertyChanged += InventorySpool_PropertyChanged;
                    }


            //throw new NotImplementedException();
        }

        private void InventorySpool_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Property Changed for {e.PropertyName}");
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
            Vendor = DataContext.FilamentContext.GetVendor(ven => ven.VendorDefnId == vendorID);
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

        public override void UpdateItem()
        {
            throw new NotImplementedException();
        }
    }
}
