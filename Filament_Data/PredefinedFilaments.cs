using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class PredefinedFilaments
    {
        public Dictionary<string,FilamentDefn> Definitions { get; private set; }
        public PredefinedFilaments()
        {
            Definitions = new Dictionary<string, FilamentDefn>()
            {
                {"Basic PLA",new FilamentDefn() },
                {"Basic ABS",new FilamentDefn(1.75m,1.04m,MaterialType.ABS) }
            };
        }
    }
}
