using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data
{
    public class DataDocument : IDocument
    {
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public List<Setting> Settings { get; set; }
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
        public ObservableCollection<InventorySpool> Inventory { get; set; }
        /// <summary>
        /// Gets or sets the prints.
        /// </summary>
        /// <value>
        /// The prints.
        /// </value>
        public ObservableCollection<PrintJobDefn> Prints { get; set; }

        [JsonIgnore]
        public bool IsModified => Vendors.IsModified||Filaments.IsModified||Spools.IsModified||Counters.IsModified||Inventory.IsModified()||Prints.IsModified()||Settings.IsModified();
        /// <summary>
        /// flag to determine if the DataDocument has been prepopulated
        /// </summary>
        internal bool PrePopulated { get; set; }
        /// <summary>
        /// flag to determine if DataDocument is prepopulating, currently hard coded to true which might stay the same even in release
        /// </summary>
        internal static bool PrePopulating { get; set; } = true;
        public DataDocument()
        {
            Counters = new CounterProvider();
            Vendors = new DefinedVendors();
            Filaments = new DefinedFilaments();
            Spools = new DefinedSpools();
            Inventory = new ObservableCollection<InventorySpool>();
            Prints = new ObservableCollection<PrintJobDefn>();
            RelinkElements();
            Initialize();
        }
        public DataDocument(bool initialize)
        {
            Counters = new CounterProvider();
            Vendors = new DefinedVendors(initialize, this);
            Filaments = new DefinedFilaments(initialize, this);
            Spools = new DefinedSpools(initialize, this);
            Inventory = new ObservableCollection<InventorySpool>();
            Prints = new ObservableCollection<PrintJobDefn>();
            RelinkElements();
        }

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
        protected void RelinkElements()
        {
            Vendors.EstablishLink(this);
            Filaments.EstablishLink(this);
            Spools.EstablishLink(this);
            RelinkItems(Inventory);
            RelinkItems(Prints);
        }
        protected void RelinkItems(IEnumerable<ILinkedItem> linkedItems)
        {
            foreach (var item in linkedItems)
            {
                item.EstablishLink(this);
            }
        }
        public void SaveDataDocument(string fileName)
        {
            JsonTransfer.SerializeObject(fileName, this);
        }

        public static DataDocument GetDataDocument(string fileName)
        {
            // do not initialize since the document will probably have similiar entries
            var tempPrepopulateFlag = PrePopulating;
            // turn off prepopulating
            PrePopulating = false;
            SerializedObject.Serializing = true;
            DataDocument document = default;
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
        public static DataDocument GetDocument(string fileName)
        {
            if (System.IO.File.Exists(fileName))
                return GetDataDocument(fileName);
            else
                return new DataDocument(true);
        }
#endif
    }
}
