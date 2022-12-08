using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    public enum FilterState:short
    {
        FilterApplied = 0x1000,
        FilterRemoved,
        FilterUpdated
    }
    public class TagFilterChangedEventArgs:EventArgs
    {
        public FilterState FilterState { get; set; }
        public Guid TagGuid { get; set; }
        public IEnumerable<string> SelectedTags { get; set; }
        public TagFilterChangedEventArgs(FilterState filterState, IEnumerable<string> data, Guid tagGuid)
        {
            FilterState = filterState;
            SelectedTags = data;
            TagGuid = tagGuid;
        }
    }
}
