using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using static System.Diagnostics.Debug;

namespace Filament_Data.JsonModel
{
    public class DefinedSpools : DocumentLinkedCollection<SpoolDefinition>
    {
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
        //protected Dictionary<string, SpoolDefinition> Definitions { get; }
        public DefinedSpools(bool prepopulate, IDocument document)
        {
            Document = document;
            if (prepopulate)
            {    
                Initialize(document);
            }
        }
        public DefinedSpools()
        {

        }
        public void Initialize(IDocument document = null)
        {
            if (document == null)
                document = Document;
            var pla = Document.Filaments.Where(fil => fil.MaterialType == MaterialType.PLA && fil.Diameter == FilamentDefn.StandardFilamentDiameter).FirstOrDefault();
            var abs = Document.Filaments.Where(fil => fil.MaterialType == MaterialType.ABS).FirstOrDefault();
            var solutechVendor = Document.Vendors.Where(ven => ven.Name == Constants._3DSolutechName).FirstOrDefault();
            var hatchBoxVendor = Document.Vendors.Where(ven => ven.Name == Constants.HatchBoxName).FirstOrDefault();
            var sunluVender = Document.Vendors.Where(ven => ven.Name == Constants.SunluName).FirstOrDefault();

            Assert(pla != null);
            Assert(abs != null);
            Assert(solutechVendor != null);
            Assert(hatchBoxVendor != null);
            Assert(sunluVender != null);

            var solutechSpool = SpoolDefinition.CreateSpoolDefinition(Constants.Solutech1KgSpoolOuterDiameter, Constants.Solutech1KgSpoolDrumDiameter, Constants.Solutech1KgSpoolWidth, pla.FilamentID, solutechVendor.VendorID, document);
            Assert(solutechSpool != null);
            Add(solutechSpool);
            var hatchBoxSpool = SpoolDefinition.CreateSpoolDefinition(Constants.HatchBox1KgSpoolOuterDiameter, Constants.HatchBox1KgSpoolDrumDiameter, Constants.HatchBox1KgSpoolWidth, pla.FilamentID, hatchBoxVendor.VendorID, document);
            Assert(hatchBoxSpool != null);
            Add(hatchBoxSpool);
            var hatchBoxABSSpool = SpoolDefinition.CreateSpoolDefinition(Constants.HatchBox1KgSpoolOuterDiameter, Constants.HatchBox1KgSpoolDrumDiameter, Constants.HatchBox1KgSpoolWidth, abs.FilamentID, hatchBoxVendor.VendorID, document);
            Assert(hatchBoxABSSpool != null);
            Add(hatchBoxABSSpool);
            var sunluPLASpool = SpoolDefinition.CreateSpoolDefinition(195, 195 - 60, 55, pla.FilamentID, sunluVender.VendorID, Document);
            Assert(sunluPLASpool != null);
            Add(sunluPLASpool);
        }
        public void Add(SpoolDefinition definition, bool establishLink)
        {
            Add(definition);
            if (establishLink)
                definition.EstablishLink(Document);
        }
        //public void EstablishLink(IDocument document)
        //{
        //    Document = document ?? throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is expected.");
        //    foreach (var item in this)
        //    {
        //        item.EstablishLink(document);
        //    }
        //}

        //void IUpdateModified.SuccessfullySaved()
        //{
        //    foreach (var item in this)
        //    {
        //        if (item is IUpdateModified updateModified)
        //            updateModified.SuccessfullySaved();
        //    }
        //    throw new NotImplementedException();
        //}
        //public override IEnumerable<KeyValuePair<string, SpoolDefinition>> Initializer()
        //{
        //    FilamentDefn defn = new FilamentDefn();
        //    yield return new KeyValuePair<string,SpoolDefinition>("3D Solutech",new SpoolDefinition(200, 81, 55, defn));
        //    yield return new KeyValuePair<string, SpoolDefinition>("Hatchbox", new SpoolDefinition(200, 81, 55, defn));
        //}
    }
}
