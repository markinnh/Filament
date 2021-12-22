using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Filament_Db
{
    public class DatabaseObject : Observable
    {
        protected override bool Set<T>(ref T target, T value,bool blockUpdate=false, [CallerMemberName] string? propertyName = null)
        {
            if (base.Set(ref target, value,blockUpdate, propertyName) && !InDataOperations && propertyName != nameof(IsModified) && !blockUpdate)
            {
                IsModified = true;
                return true;
            }
            return false;
        }
        [NotMapped]
        public virtual bool InDataOperations { get; }
        [NotMapped]
        public virtual bool CanEdit => !InDataOperations;
        [NotMapped]
        public virtual bool IsValid { get => false; }
        private bool isModified;
        [NotMapped]
        public virtual bool IsModified
        {
            get => isModified;
            set => Set<bool>(ref isModified, value);
        }
        [NotMapped]
        public virtual bool InDatabase { get => throw new NotSupportedException(); }
        /// <summary>
        /// Supports delete, default is false, each definition will need a decision made about supporting a delete.
        /// </summary>
        [NotMapped]
        public virtual bool SupportsDelete => false;
        public virtual void UpdateItem() { }

        public virtual void SetContainedModifiedState(bool state)
        {
            System.Diagnostics.Debug.WriteLine($"Unable to set the '{state}' State for {GetType().Name}");
        }
        protected bool AddedItems<TItem>(TItem item) where TItem : DatabaseObject
        {
            if (item != null)
                return !item.InDatabase && item.IsValid;
            else
                return false;
        }
        protected Func<DatabaseObject, bool> Added = (databaseObject) => !databaseObject.InDatabase && databaseObject.IsValid;
        protected Func<DatabaseObject, bool> Modified = (databaseObject) => databaseObject.IsValid && databaseObject.IsModified && databaseObject.InDatabase;

        
    }
}
