using System;
using System.Collections.Generic;

namespace Filament.WPF6.Helpers
{
    /// <summary>
    /// Allows for keyword searching in text fields
    /// </summary>
    public interface IKeywordFilter:IFilter
    {
        /// <summary>
        /// Unique signature, required for some ViewModels, prevents an apple to oranges comparison from occuring
        /// </summary>
        Guid Signature { get; set; }
        /// <summary>
        /// Flag to tell if the criteria is set, looks bad
        /// </summary>
        bool CriteriaSet { get; }
        IEnumerable<string> Keywords { get; set; }

    }
}
