using System;
using System.Collections.Generic;
using System.Text;

namespace DataDefinitions.JsonSupport
{
    
    internal interface IUpdateModified
    {
        void SuccessfullySaved();
    }
    /// <summary>
    /// restore data links to dependent objects after deserializing from json, to allow for smaller footprint
    /// </summary>
    public interface ILinked : ITrackModified
    {
        IJsonDocument Document { get; }
        /// <summary>
        /// Relink the 'objects' with the document, which is necessary to restore external references, whether it should hold a reference is yet to be determined.
        /// </summary>
        /// <param name="document">IJsonDocument</param>
        void EstablishLink(IJsonDocument document);
    }
}
