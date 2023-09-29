using DataDefinitions.Interfaces;
using System;

namespace Filament.WPF6.Helpers
{
    [Obsolete]
    public interface IFilterStatus
    {
        bool TagFilteringSupported { get; }
        bool KeywordFilteringSupported { get; }
        bool IsFilterApplied(IResolveFilter.Filters whichFilter);
    }
}
