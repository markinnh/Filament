using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
namespace Filament_Data
{
    public abstract class ObservableObject : SerializedObject, INotifyPropertyChanged
    {
        [JsonIgnore]
        protected static Dictionary<string, Dictionary<string, List<string>>> Dependents = new Dictionary<string, Dictionary<string, List<string>>>();
        [JsonIgnore]
        protected abstract bool DependenciesInitialized { get; set; }
        [JsonIgnore]
        public abstract bool HasDependencies { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is modified.  State only allowed to be modified within this class library.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Set<T>(ref T target, T value, [CallerMemberName]string propertyName = null, bool setModifiedOnChanged = true)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                Assign<T>(ref target, value, setModifiedOnChanged);
                OnPropertyChanged(propertyName);
                NotifyDependents(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void NotifyDependents(string propertyName)
        {
            if (Dependents.TryGetValue(GetType().FullName, out Dictionary<string, List<string>> dependencies))
            {
                if (dependencies.TryGetValue(propertyName, out List<string> names))
                {
                    foreach (var name in names)
                    {
                        OnPropertyChanged(name);
                    }
                }
            }
        }
        protected abstract void InitDependents();
        //protected abstract bool DocInitialized { get; }
        public ObservableObject()
        {
            if (HasDependencies && !DependenciesInitialized)
            {
                // InitDependents should set the DependenciesInitialized when the first object is created
                InitDependents();
            }
        }
    }
}
