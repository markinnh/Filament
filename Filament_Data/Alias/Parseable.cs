using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.Alias
{
    public struct Parseable
    {
        internal string Value { get; set; }
        public Parseable(string value)
        {
            Value = value;
        }
        public static explicit operator Parseable(string value)
        {
            return new Parseable(value);
        }
        public static implicit operator float(Parseable aliaser)
        {
            if (float.TryParse(aliaser.Value, out float result))
                return result;
            else
                throw GetArgumentException(aliaser.Value,typeof(float).Name, nameof(aliaser));
        }
        public static implicit operator bool(Parseable aliaser)
        {
            if (bool.TryParse(aliaser.Value, out bool result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(bool).Name, nameof(aliaser));
        }
        public static implicit operator int(Parseable aliaser)
        {
            if (int.TryParse(aliaser.Value, out int result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(int).Name, nameof(aliaser));
        }
        public static implicit operator short(Parseable aliaser)
        {
            if (short.TryParse(aliaser.Value, out short result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(short).Name, nameof(aliaser));
        }
        public static implicit operator byte(Parseable aliaser)
        {
            if (byte.TryParse(aliaser.Value, out byte result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(byte).Name, nameof(aliaser));
        }
        public static implicit operator uint(Parseable aliaser)
        {
            if (uint.TryParse(aliaser.Value, out uint result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(uint).Name, nameof(aliaser));
        }
        public static implicit operator ushort(Parseable aliaser)
        {
            if (ushort.TryParse(aliaser.Value, out ushort result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(ushort).Name, nameof(aliaser));
        }
        public static implicit operator double(Parseable aliaser)
        {
            if (double.TryParse(aliaser.Value, out double result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(double).Name, nameof(aliaser));
        }
        public static implicit operator DateTime(Parseable aliaser)
        {
            if (DateTime.TryParse(aliaser.Value, out DateTime result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(DateTime).Name, nameof(aliaser));
        }
        public static implicit operator decimal(Parseable aliaser)
        {
            if (decimal.TryParse(aliaser.Value, out decimal result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(decimal).Name, nameof(aliaser));
        }
        public static implicit operator long(Parseable aliaser)
        {
            if (long.TryParse(aliaser.Value, out long result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(long).Name, nameof(aliaser));
        }
        public static implicit operator ulong(Parseable aliaser)
        {
            if (ulong.TryParse(aliaser.Value, out ulong result))
                return result;
            else
                throw GetArgumentException(aliaser.Value, typeof(ulong).Name, nameof(aliaser));
        }

        private static ArgumentException GetArgumentException(string value,string expectedType,string paramName)
        {
            return new ArgumentException($"Unable to parse {value} into a {expectedType} value.",paramName);
        }
        public static bool operator ==(Parseable left,Parseable right)
        {
            return left.Value == right.Value;
        }
        public static bool operator !=(Parseable left,Parseable right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Parseable parseable)
                return Value == parseable.Value;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
