using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace Filament_Data.JsonModel
{
    public class DataDocument : IDocument, IUpdateModified
    {
        private bool supportingPrintGcodeFiles = false;
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public List<Setting> Settings { get; set; }=new List<Setting>();
        /// <summary>
        /// Gets or sets the counters.
        /// </summary>
        /// <value>
        /// The counters.
        /// </value>
        public CounterProvider Counters { get; set; }
        /// <summary>
        /// Gets or sets the vendors.
        /// </summary>
        /// <value>
        /// The vendors.
        /// </value>
        public DefinedVendors Vendors { get; set; }
        /// <summary>
        /// Gets or sets the filaments.
        /// </summary>
        /// <value>
        /// The filaments.
        /// </value>
        public DefinedFilaments Filaments { get; set; }
        /// <summary>
        /// Gets or sets the spools.
        /// </summary>
        /// <value>
        /// The spools.
        /// </value>
        public DefinedSpools Spools { get; set; }
        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public DocumentLinkedCollection<InventorySpool> Inventory { get; set; }
        /// <summary>
        /// Gets or sets the prints that have been executed.
        /// </summary>
        /// <value>
        /// The prints.
        /// </value>
        public ObservableCollection<PrintJobDefn> PrintJobs { get; set; }


        [JsonIgnore]
        internal DataFilters.FileInfo FileInfo { get; set; }
        /// <summary>
        /// Gets or sets the print job path.  Currently defaults to a usb drive location since that is what is required by Windows10.
        /// </summary>
        /// <value>
        /// The print job path.
        /// </value>
        public string PrintFilesPath { get; set; } = @"D:\3d printer gcode files";
        //const string filamentPattern = @";Filament used: (?<filament>[0-9]*\.[0-9]*)";
        //const string timePattern = @";TIME:(?<time>[0-9]*)";

        [JsonIgnore]
        public bool IsModified => Vendors.IsModified || Filaments.IsModified || Spools.IsModified || Counters.IsModified ||
            Inventory.IsModified || PrintJobs.IsModified() || Settings.Count(s => s.IsModified) > 0;
        /// <summary>
        /// flag to determine if the DataDocument has been prepopulated
        /// </summary>
        [JsonIgnore]
        internal bool PrePopulated { get; set; }
        /// <summary>
        /// flag to determine if DataDocument is prepopulating, currently hard coded to true which might stay the same even in release
        /// </summary>
        [JsonIgnore]
        internal static bool PrePopulating { get; set; } = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataDocument"/> class.
        /// </summary>
        public DataDocument()
        {
            Counters = new CounterProvider();
            Vendors = new DefinedVendors();
            Filaments = new DefinedFilaments();
            Spools = new DefinedSpools();
            Inventory = new DocumentLinkedCollection<InventorySpool>();
            PrintJobs = new ObservableCollection<PrintJobDefn>();
            InitFileInfo();
            RelinkElements();
            Initialize();
            InitPrintJobs();
        }

        private void InitFileInfo()
        {
            FileInfo = new DataFilters.FileInfo();
            //FileInfo.Add(filamentPattern);
            //FileInfo.Add(timePattern);
            //throw new NotImplementedException();
        }

        private void InitPrintJobs()
        {
            if (supportingPrintGcodeFiles)
                if (!string.IsNullOrEmpty(PrintFilesPath))
                {
                    if (System.IO.Directory.Exists(PrintFilesPath))
                    {
                        foreach (var filename in System.IO.Directory.GetFiles(PrintFilesPath, "*.gcode"))
                        {
                            var result = FileInfo.FindMatches<ParsedObjects.CuraHeader>(filename);
                            // currently the search is just for filament length, later it can easily be expanded to other values
                            if (result.Initialized)
                            {
                                ;
                            }
                        }
                    }
                }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataDocument"/> class.
        /// </summary>
        /// <param name="initialize">if set to <c>true</c> [initialize].</param>
        public DataDocument(bool initialize)
        {
            Counters = new CounterProvider();
            Vendors = new DefinedVendors(initialize, this);
            Filaments = new DefinedFilaments(initialize, this);
            Spools = new DefinedSpools(initialize, this);
            Inventory = new DocumentLinkedCollection<InventorySpool>();
            PrintJobs = new ObservableCollection<PrintJobDefn>();
            RelinkElements();
        }
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            if (!PrePopulated && PrePopulating)
            {

                Filaments.Initialize();
                Vendors.Initialize();
                Spools.Initialize();
                PrePopulated = true;
            }
        }
        /// <summary>
        /// Relinks the elements.
        /// </summary>
        protected void RelinkElements()
        {
            Vendors.EstablishLink(this);
            Filaments.EstablishLink(this);
            Spools.EstablishLink(this);
            Inventory.EstablishLink(this);
            RelinkItems(PrintJobs);
        }
        /// <summary>
        /// Relinks the items.
        /// </summary>
        /// <param name="linkedItems">The linked items.</param>
        protected void RelinkItems(IEnumerable<ILinked> linkedItems)
        {
            foreach (var item in linkedItems)
            {
                item.EstablishLink(this);
            }
        }
        protected static void EstablishSuccessfulSave(IEnumerable<object> trackModifieds)
        {
            foreach (var item in trackModifieds)
            {
                if (item is IUpdateModified updateModified)
                    updateModified.SuccessfullySaved();
            }
        }
        internal void SaveDataDocument(string fileName)
        {
            try
            {
                JsonTransfer.SerializeObject(fileName, this);
                ((IUpdateModified)this).SuccessfullySaved();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static DataDocument GetDataDocument(string fileName)
        {
            // do not initialize since the document will probably have similiar entries
            var tempPrepopulateFlag = PrePopulating;
            // turn off prepopulating
            PrePopulating = false;
            SerializedObject.Serializing = true;
            DataDocument document;
            try
            {
                document = JsonTransfer.DeserializeFileContents<DataDocument>(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // restore prepopulating
            PrePopulating = tempPrepopulateFlag;
            SerializedObject.Serializing = false;
            document.RelinkElements();
            return document;
        }
#if DEBUG        
        /// <summary>
        /// Gets the DataDocument.  If the file does not exist, returns a prepopulated DataDocument
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>DataDocument</returns>
        internal static DataDocument GetDocument(string fileName, bool recreateDoc = false)
        {
            if (System.IO.File.Exists(fileName) && !recreateDoc)
                return GetDataDocument(fileName);
            else
                return new DataDocument(true);
        }

        void IUpdateModified.SuccessfullySaved()
        {
            ((IUpdateModified)Vendors).SuccessfullySaved();
            ((IUpdateModified)Filaments).SuccessfullySaved();
            ((IUpdateModified)Spools).SuccessfullySaved();
            EstablishSuccessfulSave(Inventory);
            EstablishSuccessfulSave(PrintJobs);
        }
#endif
    }
}
