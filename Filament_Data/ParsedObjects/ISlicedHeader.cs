using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.ParsedObjects
{
    public interface ISlicedHeader
    {
        IEnumerable<INamedInfo> Info { get; }
    }
    public interface INamedInfo
    {
        string Name { get; }
        string Value { get; }
    }
}
