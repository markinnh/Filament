
using Filament_Db.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Diagnostics.Debug;
using System.Linq;

namespace Filament_Db.Models
{
    // TODO: Develop a UI for VendorDefn; Add, Delete, Update
    // TODO: Develop a DataObject class the sit between data classes and Observable since the data is going to be disconnected from the DbContext
    public class VendorDefn : DatabaseObject, IDataErrorInfo
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
        public override bool IsModified { get => base.IsModified || SpoolDefns?.Count(sd => sd.IsModified) > 0; set => base.IsModified = value; }
        //public const string _3DSolutechName = "3D Solutech";
        //public const string HatchBoxName = "HatchBox";
        //public const string SunluName = "Sunlu";
        //public const string DefaultVendorName = "Generic";
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

        private string? name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name
        {
            get => name;
            set => Set<string>(ref name, value, nameof(Name));
        }

        private bool foundOnAmazon;
        /// <summary>
        /// Gets or sets a value indicating whether [found on amazon].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [found on amazon]; otherwise, <c>false</c>.
        /// </value>
        public bool FoundOnAmazon
        {
            get => foundOnAmazon;
            set => Set<bool>(ref foundOnAmazon, value, nameof(FoundOnAmazon));
        }

        private string? webUrl;
        /// <summary>
        /// Gets or sets the web URL.
        /// </summary>
        /// <value>
        /// The web URL.
        /// </value>
        public string? WebUrl
        {
            get => webUrl;
            set => Set<string>(ref webUrl, value, nameof(WebUrl));
        }

        public Uri WebUri => !string.IsNullOrEmpty(webUrl) ? new Uri(webUrl, UriKind.Absolute) : new Uri("https://www.google.com");
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

        public IEnumerable<SpoolDefn> SpoolDefns { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name) && string.IsNullOrEmpty(Name))
                    return $"{nameof(Name)} is a required entry";
                else
                    return string.Empty;
            }
        }
        public VendorDefn()
        {
            Name = "Undefined";
            InDataOpsChanged += VendorDefn_InDataOpsChanged;
            SpoolDefns = new ObservableCollection<SpoolDefn>();
            if (SpoolDefns is ObservableCollection<SpoolDefn> defns)
                defns.CollectionChanged += Defns_CollectionChanged;
        }

        private void Defns_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    if (item is Observable spoolDefn)
                    {
                        spoolDefn.PropertyChanged += SpoolDefn_PropertyChanged;
                        //defn.Vendor = this;
                        //defn.VendorDefnId = VendorDefnId;
                    }
            if (!InDataOperations)
                IsModified = true;
            //throw new NotImplementedException();
        }

        private void SpoolDefn_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            WriteLine($"Property changed for : {e.PropertyName}");
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
            //throw new NotImplementedException();
        }

        ~VendorDefn()
        {
            InDataOpsChanged -= VendorDefn_InDataOpsChanged;
        }
        private void VendorDefn_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the vendor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="foundOnAmazon">if set to <c>true</c> [found on amazon].</param>
        /// <param name="url">The URL.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static VendorDefn CreateVendor(string name, bool foundOnAmazon, string url)
        {
            var result = new VendorDefn()
            {
                Name = name,
                FoundOnAmazon = foundOnAmazon,
                WebUrl = url
            };
            return result;
        }
        public override void UpdateItem()
        {
            InDataOps = true;
            FilamentContext.UpdateSpec(this);
            IsModified = false;
            InDataOps = false;
        }
        public override void SetContainedModifiedState(bool state)
        {
            if (SpoolDefns != null)
                foreach (var spec in SpoolDefns)
                    spec.IsModified = state;
        }
    }
}
