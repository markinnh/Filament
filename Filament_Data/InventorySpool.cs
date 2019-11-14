using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace Filament_Data
{
    /// <summary>
    /// An actual on-hand spool of filament, which is maintained in inventory.
    /// </summary>
    /// <seealso cref="Filament_Data.ObservableObject" />
    /// <seealso cref="Filament_Data.ILinkedItem" />
    public class InventorySpool : ObservableObject,ILinkedItem
    {
        private int spoolID;
        /// <summary>
        /// Gets or sets the spool identifier.
        /// </summary>
        /// <value>
        /// The spool identifier.
        /// </value>
        public int SpoolID
        {
            get => spoolID;
            set => Set<int>(ref spoolID, value, nameof(SpoolID));
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
        private string definitionName;
        /// <summary>
        /// Gets or sets the name of the definition.
        /// </summary>
        /// <value>
        /// The name of the definition.
        /// </value>
        public string DefinitionName
        {
            get => definitionName;
            set => Set<string>(ref definitionName, value, nameof(DefinitionName));
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

        [JsonIgnore]
        public IDocument Document { get; protected set; }
        /// <summary>
        /// Gets or sets the spool defn.
        /// </summary>
        /// <value>
        /// The spool defn.
        /// </value>
        [JsonIgnore]
        public SpoolDefinition SpoolDefn { get; set; }

        public ObservableCollection<Measurement> Measurements { get; set; }
        //[JsonIgnore]
        //protected override bool DocInitialized => Document!=null;

        public override bool HasDependencies => false;

        protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }

        public void EstablishLink(IDocument document)
        {
            if (document != null)
            {
                Document = document;

                SpoolDefn = document.Spools.Where(sp => sp.SpoolName == DefinitionName).FirstOrDefault();

                if (SpoolID == 0)
                    SpoolID = document.Counters.NextID(GetType());
                foreach (var item in Measurements)
                {
                    item.Parent = this;
                    item.EstablishLink(document);
                }
            }
            else
                throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is expected.");
        }

        protected override void InitDependents()
        {
            throw new NotImplementedException();
        }
    }
}
