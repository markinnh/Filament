using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class NamedInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public NamedInfo()
        {

        }
        public NamedInfo(string name,string value)
        {
            Name = name;
            Value = value;
        }
    }
}
