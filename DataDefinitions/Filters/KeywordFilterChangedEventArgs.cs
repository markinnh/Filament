using DataDefinitions.Interfaces;
using System.Collections.Generic;

namespace DataDefinitions.Filters
{
    public class KeywordFilterChangedEventArgs : BaseFilterChangedEventArgs<IEnumerable<string>>
    {
        public KeywordFilterChangedEventArgs(FilterAction action,IResolveFilter.Filters filter, IEnumerable<string> data) :
            base(action,filter, data)
        {

        }
    }
}
