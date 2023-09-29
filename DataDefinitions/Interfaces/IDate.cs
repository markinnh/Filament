using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    /// <summary>
    /// Supports filtering items on date
    /// </summary>
    public interface IDate
    {
        DateTime DateTime { get; }
    }
}
