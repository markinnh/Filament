using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using MyLibraryStandard.Attributes;
using System.Linq;
using MyLibraryStandard;
using DataDefinitions.Interfaces;

namespace DataDefinitions
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool Set<T>(ref T target, T value, bool blockUpdate = false, [CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (!EqualityComparer<T>.Default.Equals(target, value))
                {
                    target = value;
                    OnPropertyChanged(propertyName);

                    if (propertyName != nameof(ITrackModified.IsModified) && this is ITrackModified track && !track.InDataOperations)
                        track.IsModified = true;
                    
                    UpdateAffected(propertyName);
                    if (this is INotifyContainer notify)
                        UpdateContainer(notify, propertyName);

                    return true;
                }
                else
                    return false;
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, $"{nameof(propertyName)} was passed as a null value, which should not occur.");
                System.Diagnostics.Debug.WriteLine($"{nameof(propertyName)} was passed as a null value, which should not occur.");
                return false;
            }
        }

        private void UpdateContainer(INotifyContainer notify, string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (GetType().GetProperty(propertyName)?.GetCustomAttribute<ContainerPropertiesAffectedAttribute>() is ContainerPropertiesAffectedAttribute attribute)
                {
                    if (notify != null)
                    {
                        notify.DoNotify(new NotifyContainerEventArgs(attribute.Names));
                    }
                }
            }
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void UpdateAffected(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (GetType().GetProperty(propertyName)?.GetCustomAttributes<AffectedAttribute>()?.FirstOrDefault() is AffectedAttribute affectedAttribute)
                {
                    foreach (string name in affectedAttribute.Names)
                    {
                        OnPropertyChanged(name);
                    }
                }
            }
            else
                System.Diagnostics.Debug.WriteLine($"UpdateAffected called with {nameof(propertyName)} as an empty string.");
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
        //public void Subscribe(PropertyChangedEventHandler handler)
        //{
        //    if (PropertyChanged?.GetInvocationList().Cast<PropertyChangedEventHandler>().Contains(handler) ?? false)
        //        return;
        //    else
        //        PropertyChanged+=handler;
        //}
        //public void UnSubscribe(PropertyChangedEventHandler handler) { 
        //    if(PropertyChanged?.GetInvocationList().Cast<PropertyChangedEventHandler>().Contains(handler)??false)
        //        PropertyChanged-=handler;
        //}
        public virtual void WatchContained()
        {
            System.Diagnostics.Debug.WriteLine($"Watch contained not implemented in {GetType().Name}");
        }
        public virtual void UnWatchContained()
        {
            System.Diagnostics.Debug.WriteLine($"Unwatch contained not implemented in {GetType().Name}");
        }
        protected virtual void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine($"Contained property in {GetType().Name} is changed for {e.PropertyName}");
        }
        public void Subscribe(PropertyChangedEventHandler handler)
        {
            if (handler != null)
                if (!PropertyChanged?.GetInvocationList().Contains(handler) ?? true)
                {
                    PropertyChanged += handler;
                    System.Diagnostics.Debug.WriteLine($"Initializing PropertyChangedHandler for {GetType().Name}");
                }
        }
        public void Unsubscribe(PropertyChangedEventHandler handler)
        {
            if (handler != null)
                if (!PropertyChanged?.GetInvocationList()?.Contains(handler) ?? false)
                    PropertyChanged -= handler;
        }
        ~Observable()
        {
            if (PropertyChanged != null)
            {
                if (PropertyChanged?.GetInvocationList().Where(x => x is PropertyChangedEventHandler).Cast<PropertyChangedEventHandler>() is IEnumerable<PropertyChangedEventHandler> handlers)
                {
                    foreach (PropertyChangedEventHandler handler in handlers)
                        PropertyChanged -= handler;
                }
            }
        }
    }
}
