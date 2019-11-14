using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data
{
    public abstract class SerializedObject : IChangeTrackItem
    {
        [JsonIgnore]
        internal static bool Serializing { get; set; }
        [JsonIgnore]
        public bool IsModified { get; internal set; }
        protected virtual void Set<T>(ref T target, T value, [CallerMemberName] string propertyName = null, bool setModifiedOnChanged = true)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                Assign(ref target, value, setModifiedOnChanged);
            }

        }
        protected void Assign<T>(ref T target, T value, bool setModifiedOnChanged = true)
        {
            target = value;
            if (setModifiedOnChanged && !Serializing)
                IsModified = true;
        }
    }
}
