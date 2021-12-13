using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using MyLibraryStandard.Attributes;
using System.Linq;

namespace Filament_Data
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T target,T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(target, value))
            {
                target= value;
                UpdateAffected(propertyName);
            }
        }
        protected void OnPropertyChanged(string propertyName)=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void UpdateAffected(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (GetType().GetProperty(propertyName).GetCustomAttributes<AffectedAttribute>().FirstOrDefault() is AffectedAttribute affectedAttribute)
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
    }
}
