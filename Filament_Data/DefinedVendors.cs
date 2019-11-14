using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace Filament_Data
{
    public class DefinedVendors : ObservableCollection<VendorDefn>, ILinkedItem
    {
        public DefinedVendors(bool init, IDocument document)
        {
            if (init)
            {
                Initialize(document);
                //Add(VendorDefn.CreateVendor(VendorDefn._3DSolutechName, true, "https://www.3dsolutech.com/", document));
                //Add(VendorDefn.CreateVendor(VendorDefn.HatchBoxName, true, "https://www.hatchbox3d.com/", document));
            }
        }
        public DefinedVendors()
        {

        }
        internal void Initialize(IDocument document = null)
        {

            if (document == null)
                document = Document;
            Add(VendorDefn.CreateVendor(VendorDefn._3DSolutechName, true, "https://www.3dsolutech.com/", document));
            Add(VendorDefn.CreateVendor(VendorDefn.HatchBoxName, true, "https://www.hatchbox3d.com/", document));
            Add(VendorDefn.CreateVendor("SunLu", true, "http://www.sunlugw.com/", document));

        }
        [JsonIgnore]
        public IDocument Document { get; protected set; }
        [JsonIgnore]
        public bool IsModified
        {
            get
            {
                return this.Where(vd => vd.IsModified).Count() > 0;
            }
        }
        public void EstablishLink(IDocument document)
        {
            System.Diagnostics.Debug.Assert(document != null);
            Document = document;
            foreach (var item in this)
            {
                item.EstablishLink(document);
            }
        }
        public void Add(VendorDefn vendorDefn, bool establishLink)
        {
            if (establishLink)
                vendorDefn.EstablishLink(Document);
            base.Add(vendorDefn);
        }
    }
}
