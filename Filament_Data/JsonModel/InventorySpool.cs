using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace Filament_Data.JsonModel
{
    // TODO: develop a UI for InventorySpool
    /// <summary>
    /// An actual on-hand spool of filament, which is maintained in inventory.
    /// </summary>
    /// <seealso cref="Filament_Data.ObservableObject" />
    /// <seealso cref="Filament_Data.ILinked" />
    public class InventorySpool : DocumentBasedObject
    {
        private int inventorySpoolID;

        public int InventorySpoolID
        {
            get => inventorySpoolID;
            set => Set<int>(ref inventorySpoolID, value, nameof(InventorySpoolID));
        }

        private int spoolDefnID;
        /// <summary>
        /// Gets or sets the spool identifier.
        /// </summary>
        /// <value>
        /// The spool identifier.
        /// </value>
        public int SpoolDefnID
        {
            get => spoolDefnID;
            set => Set<int>(ref spoolDefnID, value, nameof(SpoolDefnID));
        }

        private DateTime openDate;
        /// <summary>
        /// Gets or sets the opened date.
        /// </summary>
        /// <value>
        /// The opened date.
        /// </value>
        public DateTime OpenedDate
        {
            get => openDate;
            set => Set<DateTime>(ref openDate, value, nameof(OpenedDate));
        }

        private string imageFileName;
        /// <summary>
        /// Gets or sets the name of the image file.  For special filaments.
        /// </summary>
        /// <value>
        /// The name of the image file.
        /// </value>
        public string ImageFileName
        {
            get => imageFileName;
            set => Set<string>(ref imageFileName, value, nameof(ImageFileName));
        }

        private System.Drawing.Color color;
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public System.Drawing.Color Color
        {
            get => color;
            set => Set(ref color, value, nameof(Color));
        }
        private bool preserved;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InventorySpool"/> is preserved, or stored in a sealed airtight container.
        /// </summary>
        /// <value>
        ///   <c>true</c> if preserved; otherwise, <c>false</c>.
        /// </value>
        public bool Preserved
        {
            get => preserved;
            set => Set<bool>(ref preserved, value, nameof(Preserved));
        }
        private string comments;

        public string Comments
        {
            get => comments;
            set => Set<string>(ref comments, value, nameof(Comments));
        }

        /// <summary>
        /// Gets or sets the spool defn.
        /// </summary>
        /// <value>
        /// The spool defn.
        /// </value>
        [JsonIgnore]
        public SpoolDefinition SpoolDefn { get; set; }

        public ObservableCollection<Measurement> Measurements { get; set; } = new ObservableCollection<Measurement>();
        [JsonIgnore]
        public Measurement LastMeasurement => Measurements.Count > 0 ? Measurements[Measurements.Count - 1] : null;

        public void AddMeasurement(Measurement newMeasurement)
        {
            var lastMeasure = LastMeasurement;

            if (lastMeasure != null)
            {
                if (lastMeasure.IsValidNext(newMeasurement))
                {
                    AddChildMeasurement(newMeasurement);
                }
            }
            else
            {
                AddChildMeasurement(newMeasurement);
            }
        }
        //[JsonIgnore]
        //protected override bool DocInitialized => Document!=null;

        //public override bool HasDependencies => false;

        //protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }
        public InventorySpool()
        {
            Init();
        }
        protected void AddChildMeasurement(Measurement measurement)
        {
            LinkChild(measurement);
            Measurements.Add(measurement);
        }

        protected void LinkChild(Measurement measurement)
        {
            if (measurement is IParentRef<InventorySpool> child)
            {
                child.Parent = this;
            }
        }
        public override void EstablishLink(IDocument document)
        {
            base.EstablishLink(document);

            SpoolDefn = document.Spools.Where(sp => sp.SpoolDefnID == spoolDefnID).FirstOrDefault();

            if (InventorySpoolID == 0)
            {
                InventorySpoolID = document.Counters.NextID(GetType());
                IsModified = true;
            }
            foreach (var item in Measurements)
            {
                LinkChild(item);

                //item.EstablishLink(document);
            }
        }

        //protected override void InitDependents()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
