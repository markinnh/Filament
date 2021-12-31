using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace DataDefinitions
{
    public class DatabaseObject : Observable,JsonSupport.ILinked
    {
        protected override bool Set<T>(ref T target, T value, bool blockUpdate = false, [CallerMemberName] string propertyName = null)
        {
            if (base.Set(ref target, value, blockUpdate, propertyName) && !InDataOperations && propertyName != nameof(IsModified) && !blockUpdate)
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
        [NotMapped]
        protected virtual bool HasContainedItems => false;
        [NotMapped]
        public JsonSupport.IJsonDocument Document { get;protected set; }
        public virtual void UpdateItem<TContext>() where TContext : DbContext, new()
        {
            WriteLine($"UpdateItem using base definition for {GetType().Name}");
            using (TContext context = new TContext())
            {
                SetDataOpsState(true);
                if (InDatabase)
                {
                    context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    UpdateContainedItemEntryState(context);
                    context.Update(this);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    UpdateContainedItemEntryState(context);
                    context.Add(this);
                    context.SaveChanges();
                }

                //FilamentContext.UpdateSpec(this);
                SetContainedModifiedState(false);
                //IsModified = false;
                SetDataOpsState(false);
            }
        }
        public virtual void EstablishLink(JsonSupport.IJsonDocument jsonDocument)
        {
            if(jsonDocument != null)
                Document = jsonDocument;
            else
                Document = null;
        }
        protected virtual void UpdateContainedItemEntryState<TContext>(TContext context) where TContext : DbContext
        {
            WriteLine($"SetContainedItemState not implemented for {GetType().Name}");
        }
        protected virtual void SetDataOpsState(bool state)
        {
            WriteLine($"In the base SetDataOpsState, this should be implemented by each consumer of DatabaseObject");
        }
        public virtual void SetContainedModifiedState(bool state)
        {
            WriteLine($"Unable to set the ContainedModifiedState '{state}' for {GetType().Name}, this is {(HasContainedItems?string.Empty:"not")} required.");
        }
        protected bool AddedItems<TItem>(TItem item) where TItem : DatabaseObject
        {
            if (item != null)
                return !item.InDatabase && item.IsValid;
            else
                return false;
        }
        protected Func<DatabaseObject, bool> Added = (databaseObject) => !databaseObject.InDatabase && databaseObject.IsValid && databaseObject.IsValid;
        protected Func<DatabaseObject, bool> Modified = (databaseObject) => databaseObject.IsValid && databaseObject.IsModified && databaseObject.InDatabase;


    }
}
