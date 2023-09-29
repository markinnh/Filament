using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    public interface IFilterStatus
    {
        bool IsFilterSupported(IResolveFilter.Filters whichFilter);
        bool IsFilterApplied(IResolveFilter.Filters whichFilter);
    }
}
