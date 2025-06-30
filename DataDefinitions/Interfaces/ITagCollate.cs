using System;
using System.Collections.Generic;

namespace DataDefinitions.Interfaces;

public interface ITagCollate
{
    /// <summary>
    /// Returns an enumerable of distinct tags in a collection
    /// </summary>
    /// <returns>Distinct tag collection</returns>
    public IEnumerable<WordWithOccuranceCount> DistinctTagStats
    {
        get;
    }
    Guid Signature
    {
        get;
    }
}
