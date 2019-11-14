using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class DefinedDensity : IDensity
    {
        public readonly static DefinedDensity BasicPLA = new DefinedDensity(BasicPLADensity);
        public readonly static DefinedDensity BasicABS = new DefinedDensity(BasicABSDensity);
        /// <summary>
        /// Density of basic pla (polylactic acid) density
        /// </summary>
        public const float BasicPLADensity = 1.24f;
        /// <summary>
        /// The basic abs density
        /// </summary>
        public const float BasicABSDensity = 1.04f;
        /// <summary>
        /// Density as defined, not calculated.  In grams per cc.
        /// </summary>
        public float Density { get; set; }
        public DefinedDensity(float density)
        {
            Density = density;
        }
        public DefinedDensity()
        {

        }
    }
}
