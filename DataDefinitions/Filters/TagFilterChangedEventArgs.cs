using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;

namespace DataDefinitions.Filters
{
    public class TagFilterChangedEventArgs : BaseFilterChangedEventArgs<IEnumerable<string>>
    {
        public Guid TagGuid { get; set; }
        public TagFilterChangedEventArgs(FilterAction actions,IResolveFilter.Filters filter, IEnumerable<string> data, Guid guid) :
            base(actions,filter, data)
        {
            TagGuid = guid;
        }
    }

}
