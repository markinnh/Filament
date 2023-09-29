using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class ShowAllFilterChangedEventArgs : BaseFilterChangedEventArgs<bool>
    {
        public ShowAllFilterChangedEventArgs(FilterAction action, IResolveFilter.Filters filter, bool showAll) : base(action, filter, showAll) { }
    }
}
