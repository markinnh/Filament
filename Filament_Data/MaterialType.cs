using System.ComponentModel;

namespace Filament_Data
{
    public enum MaterialType
    {
        /// <summary>
        /// Polylactic acid
        /// </summary>
        [Description("Polylactic acid")]
        PLA = 0x8000,
        /// <summary>
        /// acrylonitrile-butadiene-styrene
        /// </summary>
        [Description("acrylonitrile-butadiene-styrene")]
        ABS,
        /// <summary>
        /// Polyethylene Terephthalate Glycol
        /// </summary>
        [Description("Polyethylene Terephthalate Glycol")]
        PETG,
        Nylon,
        /// <summary>
        /// Thermoplastic Elastomer
        /// </summary>
        [Description("Thermoplastic Elastomer")]
        TPE,
        /// <summary>
        /// Polycarbonate
        /// </summary>
        [Description("Polycarbonate")]
        PC,
        Wood,
        Metal,
        Bio,
        Conductive,
        /// <summary>
        /// Glow In The Dark
        /// </summary>
        [Description("Glow In The Dark (PLA)")]
        GID_PLA,
        Magnetic,
        ColorChanging,
        ClayCeramic
    }
}
