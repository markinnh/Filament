using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    public abstract class DocumentBasedObject : ObservableObject, ILinked
    {
        [JsonIgnore]
        public IDocument Document { get; internal set; }

        public virtual void EstablishLink(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document), $"EstablishLink expects a valid reference to initialize the object.");
        }
    }
}
