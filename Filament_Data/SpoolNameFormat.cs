using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public struct SpoolNameFormat
    {
        public MaterialType Material { get; set; }
        public string Manufacturer { get; set; }
        public SpoolNameFormat(MaterialType material,string manufacturer)
        {
            Material = material;
            Manufacturer = manufacturer;
        }
        public override string ToString()
        {
            return $"{Manufacturer} - {Material}";
        }
    }
}
