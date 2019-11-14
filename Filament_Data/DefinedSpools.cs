using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
namespace Filament_Data
{
    public class DefinedSpools : List<SpoolDefinition>, ILinkedItem
    {
        [JsonIgnore]
        public IDocument Document { get; protected set; }
        [JsonIgnore]
        public bool IsModified
        {
            get
            {
                return this.Where(sd => sd.IsModified).Count() > 0;
            }
        }
        //protected Dictionary<string, SpoolDefinition> Definitions { get; }
        public DefinedSpools(bool prepopulate, IDocument document)
        {
            if (prepopulate)
            {
                Document = document;
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
            var pla = Document.Filaments.Lookup(fil => fil.MaterialType == MaterialType.PLA && fil.Diameter == FilamentDefn.StandardFilamentDiameter).FirstOrDefault();
            var abs = Document.Filaments.Lookup(fil => fil.MaterialType == MaterialType.ABS).FirstOrDefault();
            var solutechVendor = Document.Vendors.Where(ven => ven.Name == VendorDefn._3DSolutechName).FirstOrDefault();
            var hatchBoxVendor = Document.Vendors.Where(ven => ven.Name == VendorDefn.HatchBoxName).FirstOrDefault();
            System.Diagnostics.Debug.Assert(pla != null);
            System.Diagnostics.Debug.Assert(abs != null);
            System.Diagnostics.Debug.Assert(solutechVendor != null);
            System.Diagnostics.Debug.Assert(hatchBoxVendor != null);
            var solutechSpool = SpoolDefinition.CreateSpoolDefinition(SpoolDefinition.Solutech1KgSpoolOuterDiameter, SpoolDefinition.Solutech1KgSpoolInnerDiameter, SpoolDefinition.Solutech1KgSpoolWidth, pla.FilamentID, solutechVendor.VendorID, document);
            System.Diagnostics.Debug.Assert(solutechSpool != null);
            Add(solutechSpool);
            var hatchBoxSpool = SpoolDefinition.CreateSpoolDefinition(SpoolDefinition.HatchBox1KgSpoolOuterDiameter, SpoolDefinition.HatchBox1KgSpoolInnerDiameter, SpoolDefinition.HatchBox1KgSpoolWidth, pla.FilamentID, hatchBoxVendor.VendorID, document);
            System.Diagnostics.Debug.Assert(hatchBoxSpool != null);
            Add(hatchBoxSpool);
            var hatchBoxABSSpool = SpoolDefinition.CreateSpoolDefinition(SpoolDefinition.HatchBox1KgSpoolOuterDiameter, SpoolDefinition.HatchBox1KgSpoolInnerDiameter, SpoolDefinition.HatchBox1KgSpoolWidth, abs.FilamentID, hatchBoxVendor.VendorID, document);
            System.Diagnostics.Debug.Assert(hatchBoxABSSpool != null);
            Add(hatchBoxABSSpool);

        }
        public void Add(SpoolDefinition definition, bool establishLink)
        {
            Add(definition);
            if (establishLink)
                definition.EstablishLink(Document);
        }
        public void EstablishLink(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is expected.");
            foreach (var item in this)
            {
                item.EstablishLink(document);
            }
        }
        //public override IEnumerable<KeyValuePair<string, SpoolDefinition>> Initializer()
        //{
        //    FilamentDefn defn = new FilamentDefn();
        //    yield return new KeyValuePair<string,SpoolDefinition>("3D Solutech",new SpoolDefinition(200, 81, 55, defn));
        //    yield return new KeyValuePair<string, SpoolDefinition>("Hatchbox", new SpoolDefinition(200, 81, 55, defn));
        //}
    }
}
