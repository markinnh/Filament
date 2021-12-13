using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    /// <summary>
    /// Setting, which is stored with the file.  
    /// </summary>
    /// <seealso cref="Filament_Data.SerializedObject" />
    public class Setting:SerializedObject
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        private string name;

        public string Name
        {
            get => name;
            set => Set<string>(ref name, value, nameof(Name));
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        private string settingValue;

        public string Value
        {
            get => settingValue;
            set => Set<string>(ref settingValue, value, nameof(Value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public Setting(string name, object value)
        {
            Name = name;
            Set(value);
            //Value = value.ToString();
        }

        
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator bool(Setting setting)
        {
            if (bool.TryParse(setting.Value, out bool result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "bool"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Int16"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator short(Setting setting)
        {
            if (short.TryParse(setting.Value, out short result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "short"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator int(Setting setting)
        {
            if (int.TryParse(setting.Value, out int result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "int"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator long(Setting setting)
        {
            if (long.TryParse(setting.Value, out long result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "long"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator DateTime(Setting setting)
        {
            if (DateTime.TryParse(setting.Value, out DateTime result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, nameof(DateTime)));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator decimal(Setting setting)
        {
            if (decimal.TryParse(setting.Value, out decimal result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "decimal"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator float(Setting setting)
        {
            if (float.TryParse(setting.Value, out float result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "float"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator double(Setting setting)
        {
            if (double.TryParse(setting.Value, out double result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "double"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="Guid"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException">The implicit operator is not supported for the Guid type.</exception>
        public static implicit operator Guid(Setting setting)
        {
            if (Guid.TryParse(setting.Value, out Guid result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "Guid"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.UInt16"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator ushort(Setting setting)
        {
            if (ushort.TryParse(setting.Value, out ushort result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "ushort"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator uint(Setting setting)
        {
            if (uint.TryParse(setting.Value, out uint result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "uint"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.UInt64"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator ulong(Setting setting)
        {
            if (ulong.TryParse(setting.Value, out ulong result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "ulong"));
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static implicit operator byte(Setting setting)
        {
            if (byte.TryParse(setting.Value, out byte result))
                return result;
            else
                throw new NotSupportedException(BuildImplicitExceptionString(setting.Value, "byte"));
        }
        
        /// <summary>
        /// Performs an implicit conversion from <see cref="Setting"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(Setting setting)
        {
            return setting.Value;
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(object value)
        {
            if (value is string str)
                Value = str;
            else
                Value = value.ToString();
        }
        private static string BuildImplicitExceptionString(string value, string expectedType)
        {
            return $"Parsing {value} to a {expectedType} is not supported";
        }
    }
}
