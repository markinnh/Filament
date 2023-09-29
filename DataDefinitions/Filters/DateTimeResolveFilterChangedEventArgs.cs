using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class DateTimeFilterChangedEventArgs:BaseFilterChangedEventArgs<DateTimeResolve>
    {
        public DateTimeFilterChangedEventArgs(FilterAction actions,IResolveFilter.Filters filter,DateTimeResolve resolve) : base(actions,filter, resolve)
        {

        }
    }
}
