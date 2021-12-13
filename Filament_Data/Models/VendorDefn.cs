using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Filament_Data.Models
{
    // TODO: Develop a UI for VendorDefn; Add, Delete, Update
    public class VendorDefn : Observable, IDataErrorInfo
    {
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

        private string name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
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

        private string webUrl;
        /// <summary>
        /// Gets or sets the web URL.
        /// </summary>
        /// <value>
        /// The web URL.
        /// </value>
        public string WebUrl
        {
            get => webUrl;
            set => Set<string>(ref webUrl, value, nameof(WebUrl));
        }
        [JsonIgnore]
        public Uri WebUri => new Uri(webUrl, UriKind.Absolute);
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
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }
        //[JsonIgnore]
        //public override bool HasDependencies => false;

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

        //protected override void InitDependents()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
