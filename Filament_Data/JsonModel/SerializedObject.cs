using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    public abstract class SerializedObject : ITrackModified, IUpdateModified
    {
        [JsonIgnore]
        internal static bool Serializing { get; set; }
        protected bool isModified = false;
        [JsonIgnore]
        public virtual bool IsModified { get=>isModified; internal set=> isModified=value; }

        protected virtual void Set<T>(ref T target, T value, [CallerMemberName] string propertyName = null, bool setModifiedOnChanged = true)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                AssignAndSetModified(ref target, value, setModifiedOnChanged);
            }

        }
        protected void AssignAndSetModified<T>(ref T target, T value, bool setModifiedOnChanged = true)
        {
            target = value;
            if (setModifiedOnChanged && !Serializing)
                IsModified = true;
        }

        void IUpdateModified.SuccessfullySaved() => IsModified = false;
    }
}
