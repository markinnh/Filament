using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class FilterChangedEventArgs : EventArgs
    {
        public FilterAction Actions { get; set; }
        public IResolveFilter.Filters Filter { get; set; }
        //public IEnumerable<string> Words { get; set; }
        public FilterChangedEventArgs(FilterAction actions,IResolveFilter.Filters filter)
        {
            Actions = actions;
            Filter = filter;
            //FilterActions = filterState;
            //Words = data;
            //TagGuid = tagGuid;
        }
    }
}
