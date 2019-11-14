using System;
//using System.Text.Json.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace Filament_Data
{
    public class SpoolDefinition : ObservableObject, ILinkedItem
    {
        public const float StandardSpoolLoad = 1.0f;
        public const short HatchBox1KgSpoolOuterDiameter = 199;
        public const short HatchBox1KgSpoolInnerDiameter = 77;
        public const short HatchBox1KgSpoolWidth = 63;
        public const short Solutech1KgSpoolOuterDiameter = 200;
        public const short Solutech1KgSpoolInnerDiameter = 80;
        public const short Solutech1KgSpoolWidth = 55;
        private float spoolDiameter;
        /// <summary>
        /// Gets or sets the spool diameter.
        /// </summary>
        /// <value>
        /// The spool diameter.
        /// </value>
        public float SpoolDiameter { get => spoolDiameter; set => Set<float>(ref spoolDiameter, value, nameof(SpoolDiameter)); }
        private float minimumDiameter;
        /// <summary>
        /// Gets or sets the minimum diameter.
        /// </summary>
        /// <value>
        /// The minimum diameter.
        /// </value>
        public float MinimumDiameter { get => minimumDiameter; set => Set<float>(ref minimumDiameter, value, nameof(MinimumDiameter)); }
        private int filamentID;

        public int FilamentID
        {
            get => filamentID;
            set => Set<int>(ref filamentID, value, nameof(FilamentID));
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

        private float spoolWidth;
        /// <summary>
        /// Gets or sets the width of the spool.
        /// </summary>
        /// <value>
        /// The width of the spool.
        /// </value>
        public float SpoolWidth
        {
            get => spoolWidth;
            set => Set<float>(ref spoolWidth, value, nameof(SpoolWidth));
        }
        private float weight=StandardSpoolLoad;
        /// <summary>
        /// Gets or sets the weight.  In Kg, default is 1Kg.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public float Weight
        {
            get => weight;
            set => Set<float>(ref weight, value, nameof(Weight));
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

        [JsonIgnore]
        public IDocument Document { get; protected set; }
        /// <summary>
        /// Gets the name of the spool.
        /// </summary>
        /// <value>
        /// The name of the spool.
        /// </value>
        public string SpoolName
        {
            get
            {
                //SpoolNameFormat format = new SpoolNameFormat(Filament.MaterialType, Vendor?.Name ?? Constants.DefaultVendorName);
                return $"{Vendor?.Name ?? VendorDefn.DefaultVendorName} - {Filament.MaterialType} {Filament.Diameter}mm {Weight}Kg";
            }
        }
        //[JsonIgnore]
        //protected override bool DocInitialized => Document!=null;
        [JsonIgnore]
        protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override bool HasDependencies => false;

        //internal SpoolDefinition(float spoolDiameter, float mimumumDiameter, float filamentDiameter, float spoolWidth)
        //{
        //    SpoolDiameter = spoolDiameter;
        //    MinimumDiameter = mimumumDiameter;
        //    FilamentDiameter = filamentDiameter;
        //    SpoolWidth = spoolWidth;
        //    Verified = true;
        //}
        internal SpoolDefinition(float spoolDiameter, float minimumDiameter, float spoolWidth, int filamentID, int vendorID, bool verified = true)
        {
            SpoolDiameter = spoolDiameter;
            MinimumDiameter = minimumDiameter;
            //FilamentDiameter = filamentDiameter;
            FilamentID = filamentID;
            VendorID = vendorID;
            SpoolWidth = spoolWidth;
            Verified = verified;
        }

        public void EstablishLink(IDocument document)
        {

            if (document != null)
            {

                Vendor = document.Vendors.Where(ven => ven.VendorID == VendorID).FirstOrDefault();
                System.Diagnostics.Debug.Assert(Vendor != null);



                if (document.Filaments.TryGetValue(FilamentID, out FilamentDefn filamentDefn))
                    Filament = filamentDefn;
                System.Diagnostics.Debug.Assert(Filament != null);

                Document = document;
            }
            else
                throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is expected.");
        }
        public static SpoolDefinition CreateSpoolDefinition(float spoolDiameter, float minimumDiameter, float spoolWidth, int filamentID, int vendorID, IDocument document, bool verified = true)
        {
            var result = new SpoolDefinition(spoolDiameter, minimumDiameter, spoolWidth, filamentID, vendorID, verified);
            System.Diagnostics.Debug.Assert(result != null);
            result.EstablishLink(document);
            return result;
        }

        protected override void InitDependents()
        {
            throw new NotImplementedException();
        }
    }
}
