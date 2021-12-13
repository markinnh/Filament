using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using MyLibraryStandard.Attributes;

namespace Filament_Data.JsonModel
{
    /// <summary>
    /// implements the INotifyPropertyChanged interface, also supports json serialization and maintaining a modified state, including notifying ui on state change
    /// </summary>
    public abstract class ObservableObject : SerializedObject, INotifyPropertyChanged
    {
        //[JsonIgnore]
        //protected static Dictionary<string, Dictionary<string, List<string>>> Dependents = new Dictionary<string, Dictionary<string, List<string>>>();
        //[JsonIgnore]
        //protected abstract bool DependenciesInitialized { get; set; }
        //[JsonIgnore]
        //public abstract bool HasDependencies { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is modified.  State only allowed to be modified within this class library.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public override bool IsModified { get => base.IsModified; internal set => Set(ref isModified, value, setModifiedOnChanged: false); }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Set<T>(ref T target, T value, [CallerMemberName] string propertyName = null, bool setModifiedOnChanged = true)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                AssignAndSetModified<T>(ref target, value, setModifiedOnChanged);
                OnPropertyChanged(propertyName);
                UpdateAffected(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void UpdateAffected(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (this.GetType().GetProperty(propertyName).GetCustomAttributes<AffectedAttribute>().FirstOrDefault() is AffectedAttribute affectedAttribute)
                {
                    foreach (string name in affectedAttribute.Names)
                    {
                        OnPropertyChanged(name);
                    }
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("UpdateAffected called with an empty string.");
            //if (Dependents.TryGetValue(GetType().FullName, out Dictionary<string, List<string>> dependencies))
            //{
            //    if (dependencies.TryGetValue(propertyName, out List<string> names))
            //    {
            //        foreach (var name in names)
            //        {
            //            OnPropertyChanged(name);
            //        }
            //    }
            //}
        }
        //protected abstract void InitDependents();
        //protected abstract bool DocInitialized { get; }
        protected void Init()
        {
            //if (HasDependencies && !DependenciesInitialized)
            //{
            //    // InitDependents should set the DependenciesInitialized when the first object is created
            //    InitDependents();
            //}
        }
    }
}
