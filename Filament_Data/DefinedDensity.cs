using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.JsonModel
{
    // TODO: Develop a UI that works for DefinedDensity, or AverageMeasuredDensity, it should be part of FilamentDefn UI
    /// <summary>
    /// A struct of a double, allows encapsulating some program logic around defined density, also stores some constants
    /// </summary>
    /// <seealso cref="Filament_Data.IDensity" />
    /// <seealso cref="System.IEquatable{Filament_Data.IDensity}" />
    /// <seealso cref="System.IComparable{Filament_Data.IDensity}" />
    public class DefinedDensity : ObservableObject, IDensity, IEquatable<IDensity>, IComparable<IDensity>
    {
        //public readonly static DefinedDensity BasicPLA = new DefinedDensity(BasicPLADensity);
        //public readonly static DefinedDensity BasicABS = new DefinedDensity(BasicABSDensity);

        private double density;
        /// <summary>
        /// Density as defined, not calculated.  In grams per cc.
        /// </summary>
        public double Density { get=>density; set=>Set(ref density,value); }
        public DefinedDensity(double density)
        {
            Density = density;
        }
        public DefinedDensity()
        {

        }
        public override bool Equals(object obj)
        {
            if (obj is IDensity density)
                return this == density;
            else
                return false;
        }

        public override int GetHashCode() => Density.GetHashCode();

        //public bool Equals(double other)
        //{
        //    return this.Equals(other);
        //}

        //public int CompareTo(double other)
        //{
        //    if (Density < other)
        //        return -1;
        //    else if (Density == other)
        //        return 0;
        //    else
        //        return 1;
        //}

        public bool Equals(IDensity other)
        {
            return this == other;
            throw new NotImplementedException();
        }

        public int CompareTo(IDensity other)
        {
            if (Density < other.Density)
                return -1;
            else if (Density == other.Density)
                return 0;
            else
                return 1;

            //throw new NotImplementedException();
        }

        public static implicit operator double(DefinedDensity definedDensity) => definedDensity.Density;

        public static bool operator ==(DefinedDensity definedDensity, IDensity density) => density == null ? false : density.Density == definedDensity.Density;
        public static bool operator !=(DefinedDensity defined, IDensity density) => !(defined == density);
        public static implicit operator DefinedDensity(double density)
        {
            return new DefinedDensity(density);
        }
        //public static bool operator ==(DefinedDensity left, IDensity density)
        //{
        //    return left.Density == density.Density;
        //}
        //public static bool operator !=(DefinedDensity left, IDensity density)
        //{
        //    return left.Density != density.Density;
        //}
        public static bool operator ==(DefinedDensity left, DefinedDensity right)
        {
            return left?.Density == right?.Density;
        }
        public static bool operator !=(DefinedDensity left, DefinedDensity right)
        {
            return !(left == right);
        }
        public static bool operator <(DefinedDensity left, DefinedDensity right) => left.Density < right.Density;
        public static bool operator >(DefinedDensity left, DefinedDensity right) => left.Density > right.Density;
        public static bool operator <=(DefinedDensity left, DefinedDensity right) => left.Density <= right.Density;
        public static bool operator >=(DefinedDensity left, DefinedDensity right) => left.Density >= right.Density;
        public static bool operator <(DefinedDensity defined, IDensity density) => ReferenceEquals(density, null) ? false : defined.Density < density.Density;
        public static bool operator >(DefinedDensity defined, IDensity density) => ReferenceEquals(density, null) ? false : defined.Density > density.Density;
        public static bool operator <=(DefinedDensity defined, IDensity density) => ReferenceEquals(density, null) ? false : defined.Density <= density.Density;
        public static bool operator >=(DefinedDensity defined, IDensity density) => ReferenceEquals(density, null) ? false : defined.Density >= density.Density;
    }
}
