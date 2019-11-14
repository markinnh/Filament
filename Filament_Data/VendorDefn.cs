using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace Filament_Data
{

    public class VendorDefn : ObservableObject, ILinkedItem
    {
        public const string _3DSolutechName = "3D Solutech";
        public const string HatchBoxName = "HatchBox";
        public const string DefaultVendorName = "Generic";
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
        public IDocument Document { get; protected set; }
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        [JsonIgnore]
        protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override bool HasDependencies => false;
        /// <summary>
        /// Establishes the link.
        /// </summary>
        /// <param name="document">The document.</param>
        public void EstablishLink(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is expected.");
            if (VendorID == 0)
                VendorID = document.Counters.NextID(GetType());

        }
        /// <summary>
        /// Creates the vendor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="foundOnAmazon">if set to <c>true</c> [found on amazon].</param>
        /// <param name="url">The URL.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static VendorDefn CreateVendor(string name, bool foundOnAmazon, string url, IDocument document)
        {
            var result = new VendorDefn() { Name = name, FoundOnAmazon = foundOnAmazon, WebUrl = url };
            result.EstablishLink(document);
            return result;
        }

        protected override void InitDependents()
        {
            throw new NotImplementedException();
        }
    }
}
