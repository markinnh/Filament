using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Filament_Db
{
    public  class DatabaseObject : Observable
    {
        protected override bool Set<T>(ref T target, T value, [CallerMemberName] string? propertyName = null)
        {
            if (base.Set(ref target, value, propertyName) && !InDataOperations && propertyName != nameof(IsModified))
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

        private bool isModified;
        [NotMapped]
        public virtual bool IsModified
        {
            get => isModified;
            set => Set<bool>(ref isModified, value);
        }

        private bool isIntrinsic;

        public bool IsIntrinsic
        {
            get => isIntrinsic;
            set => Set<bool>(ref isIntrinsic, value);
        }

        public virtual void UpdateItem() { }

        public virtual void SetContainedModifiedState(bool state)
        {
            System.Diagnostics.Debug.WriteLine($"Unable to set the Modified State for {GetType().Name}");
        }
    }
}
