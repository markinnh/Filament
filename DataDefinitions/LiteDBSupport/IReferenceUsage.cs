using System;
using System.Collections.Generic;

namespace DataDefinitions.LiteDBSupport
{
    public interface IReferenceUsage<TRef> where TRef : DataObject
    {
        IEnumerable<DataObject> GetReferences(TRef item);
    }
}
