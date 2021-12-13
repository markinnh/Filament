using System;
//using System.Text.Json.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    // TODO: Develop a UI for SpoolDefinition; Add, Update, Delete
    public class SpoolDefinition : DocumentBasedObject
    {
        private double spoolDiameter;
        /// <summary>
        /// Gets or sets the spool diameter.
        /// </summary>
        /// <value>
        /// The spool diameter.
        /// </value>
        public double SpoolDiameter { get => spoolDiameter; set => Set<double>(ref spoolDiameter, value, nameof(SpoolDiameter)); }
        private double minimumDiameter;
        /// <summary>
        /// Gets or sets the minimum diameter.
        /// </summary>
        /// <value>
        /// The minimum diameter.
        /// </value>
        public double DrumDiameter { get => minimumDiameter; set => Set<double>(ref minimumDiameter, value, nameof(DrumDiameter)); }
        private int filamentID;

        public int FilamentID
        {
            get => filamentID;
            set => Set<int>(ref filamentID, value, nameof(FilamentID));
        }

        private int spoolDefnID;

        public int SpoolDefnID
        {
            get => spoolDefnID;
            set => Set<int>(ref spoolDefnID, value, nameof(SpoolDefnID));
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
            set => Set<bool>(ref stopUsing, value, nameof(StopUsing));
        }


        private FilamentDefn filament;
        /// <summary>
        /// Gets or sets the filament.
        /// </summary>
        /// <value>
        /// The filament.
        /// </value>
        [JsonIgnore]
        public FilamentDefn Filament
        {
            get => filament;
            set => Set<FilamentDefn>(ref filament, value, nameof(Filament));
        }

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
            set => Set<double>(ref spoolWidth, value, nameof(SpoolWidth));
        }
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
            set => Set<double>(ref weight, value, nameof(Weight));
        }

        private bool verified;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SpoolDefinition"/> has it's dimensions verified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verified; otherwise, <c>false</c>.
        /// </value>
        public bool Verified
        {
            get => verified;
            set => Set<bool>(ref verified, value, nameof(Verified));
        }

        private int vendorID;
        /// <summary>
        /// Gets or sets the vendor identifier.
        /// </summary>
        /// <value>
        /// The vendor identifier.
        /// </value>
        public int VendorID
        {
            get => vendorID;
            set => Set<int>(ref vendorID, value, nameof(VendorID));
        }

        //public VendorDefn Vendor { get; set; }
        private VendorDefn vendor;
        /// <summary>
        /// Gets or sets the vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>
        [JsonIgnore]
        public VendorDefn Vendor
        {
            get => vendor;
            set => Set<VendorDefn>(ref vendor, value, nameof(Vendor));
        }

        /// <summary>
        /// Gets the name of the spool.
        /// </summary>
        /// <value>
        /// The name of the spool.
        /// </value>
        [JsonIgnore]
        public string SpoolName=>$"{Vendor?.Name ?? Constants.DefaultVendorName} - {Filament.MaterialType} {Filament.Diameter}mm {Weight}Kg";
        //[JsonIgnore]
        //protected override bool DocInitialized => Document!=null;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }
        //[JsonIgnore]
        //public override bool HasDependencies => false;

        //internal SpoolDefinition(double spoolDiameter, double mimumumDiameter, double filamentDiameter, double spoolWidth)
        //{
        //    SpoolDiameter = spoolDiameter;
        //    MinimumDiameter = mimumumDiameter;
        //    FilamentDiameter = filamentDiameter;
        //    SpoolWidth = spoolWidth;
        //    Verified = true;
        //}
        public SpoolDefinition()
        {
            Init();
        }
        internal SpoolDefinition(double spoolDiameter, double minimumDiameter, double spoolWidth, int filamentID, int vendorID, bool verified = true)
        {
            Init();
            SpoolDiameter = spoolDiameter;
            DrumDiameter = minimumDiameter;
            //FilamentDiameter = filamentDiameter;
            FilamentID = filamentID;
            VendorID = vendorID;
            SpoolWidth = spoolWidth;
            Verified = verified;
        }

        public override void EstablishLink(IDocument document)
        {

            Document = document;
            base.EstablishLink(document);
            Vendor = document.Vendors.Where(ven => ven.VendorID == VendorID).FirstOrDefault();

            System.Diagnostics.Debug.Assert(Vendor != null);


            Filament = document.Filaments.FirstOrDefault(fil => fil.FilamentID == FilamentID);

            System.Diagnostics.Debug.Assert(Filament != null);

            if (SpoolDefnID == 0)
                SpoolDefnID = document.Counters.NextID(this.GetType());
        }
        public static SpoolDefinition CreateSpoolDefinition(double spoolDiameter, double minimumDiameter, double spoolWidth, int filamentID, int vendorID, IDocument document, bool verified = true)
        {
            var result = new SpoolDefinition(spoolDiameter, minimumDiameter, spoolWidth, filamentID, vendorID, verified);
            System.Diagnostics.Debug.Assert(result != null);
            result.EstablishLink(document);
            return result;
        }

        //protected override void InitDependents()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
