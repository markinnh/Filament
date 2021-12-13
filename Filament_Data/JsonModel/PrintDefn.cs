using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Filament_Data.JsonModel
{
    public class PrintJobDefn : DocumentBasedObject
    {
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
        private string comment;
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment
        {
            get => comment;
            set => Set<string>(ref comment, value, nameof(Comment));
        }
        private string fileName;
        /// <summary>
        /// Gets or sets the name of the file for an image of completed print.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get => fileName;
            set => Set<string>(ref fileName, value, nameof(FileName));
        }

        private float gramsPerPrint;
        /// <summary>
        /// Gets or sets the grams per print.
        /// </summary>
        /// <value>
        /// The grams per print.
        /// </value>
        public float GramsPerPrint
        {
            get => gramsPerPrint;
            set => Set<float>(ref gramsPerPrint, value, nameof(GramsPerPrint));
        }
        private float lengthPerPrint;
        /// <summary>
        /// Gets or sets the length per print in meters.
        /// </summary>
        /// <value>
        /// The length per print.
        /// </value>
        public float LengthPerPrint
        {
            get => lengthPerPrint;
            set => Set<float>(ref lengthPerPrint, value, nameof(LengthPerPrint));
        }

        private int speed;
        /// <summary>
        /// Gets or sets the job speed in mm/sec.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public int Speed
        {
            get => speed;
            set => Set<int>(ref speed, value, nameof(Speed));
        }
        private float layerHeight;
        /// <summary>
        /// Gets or sets the height of the layer.
        /// </summary>
        /// <value>
        /// The height of the layer.
        /// </value>
        public float LayerHeight
        {
            get => layerHeight;
            set => Set<float>(ref layerHeight, value, nameof(LayerHeight));
        }

        private TimeSpan printTime;
        /// <summary>
        /// Gets or sets the print time.
        /// </summary>
        /// <value>
        /// The print time.
        /// </value>
        public TimeSpan PrintTime
        {
            get => printTime;
            set => Set<TimeSpan>(ref printTime, value, nameof(PrintTime));
        }

        //[JsonIgnore]
        //public override bool HasDependencies => false;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => true; set => throw new NotImplementedException(); }

        //protected override bool DocInitialized => Document != null;

        public override void EstablishLink(IDocument document)
        {
            base.EstablishLink(document);
        }
        public PrintJobDefn()
        {
            Init();
        }
        //protected override void InitDependents()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
