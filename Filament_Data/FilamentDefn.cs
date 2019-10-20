using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    /// <summary>
    /// Default FilamentDefn is BasicPLA
    /// </summary>
    public class FilamentDefn
    {
        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
        public decimal Diameter { get; set; } = 1.75m;
        /// <summary>
        /// Gets or sets the filament density.  Expected in gm/cc, default is 1.24 for generic PLA.
        /// </summary>
        /// <value>
        /// The filament density.
        /// </value>
        public decimal Density { get; set; } = 1.24m;
        /// <summary>
        /// Gets or sets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        public MaterialType MaterialType { get; set; } = MaterialType.PLA;
        public FilamentDefn(decimal diameter,decimal density,MaterialType materialType)
        {
            Diameter = diameter;
            Density = density;
            MaterialType = materialType;
        }
        public FilamentDefn() { }
    }
}
