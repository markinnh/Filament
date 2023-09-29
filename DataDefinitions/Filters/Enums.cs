using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    [Flags]
    public enum FilterAction : short
    {
        Apply = 0x1000,
        Remove = 0x2000,
        Update = 0x1
    }
    internal class Enums
    {
    }
}
