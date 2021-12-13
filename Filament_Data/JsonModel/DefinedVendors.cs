using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace Filament_Data.JsonModel
{
    public class DefinedVendors : DocumentLinkedCollection<VendorDefn>
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


            Document = document;
            Add(VendorDefn.CreateVendor(Constants._3DSolutechName, true, "https://www.3dsolutech.com/", document));
            Add(VendorDefn.CreateVendor(Constants.HatchBoxName, true, "https://www.hatchbox3d.com/", document));
            Add(VendorDefn.CreateVendor(Constants.SunluName, true, "http://www.sunlugw.com/", document));

        }
        //[JsonIgnore]
        //public IDocument Document { get; protected set; }
        //[JsonIgnore]
        //public bool IsModified
        //{
        //    get
        //    {
        //        return this.IsModified();
        //    }
        //}
        //public void EstablishLink(IDocument document)
        //{
        //    System.Diagnostics.Debug.Assert(document != null);
        //    Document = document;
        //    foreach (var item in this)
        //    {
        //        item.EstablishLink(document);
        //    }
        //}
        public void Add(VendorDefn vendorDefn, bool establishLink)
        {
            if (establishLink)
                vendorDefn.EstablishLink(Document);
            base.Add(vendorDefn);
        }

        //void IUpdateModified.SuccessfullySaved()
        //{
        //    foreach (var item in this)
        //    {
        //        if(item is IUpdateModified update)
        //        update.SuccessfullySaved();
        //    }
        //    //throw new NotImplementedException();
        //}
    }
}
