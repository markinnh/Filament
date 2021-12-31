using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HintLib
{
    public class HashedObject : BaseObject
    {
        [JsonIgnore]
        public override bool IsModified { get => base.IsModified && WatchingCrc ? CurrentCrc != SavedCrc : false; set => base.IsModified = value; }
        protected static event EventHandler? PrepareToSerialize;
        [JsonIgnore, NotMapped]
        public virtual bool InDatabase { get; } = false;
        [JsonIgnore, NotMapped]
        protected virtual bool WatchingCrc { get; } = true;
        [JsonIgnore,NotMapped]
        public virtual string DisplayType { get; } =string.Empty;
        public int SavedCrc { get; set; }
        /// <summary>
        /// Calculates the Crc for the specified object, when the function is not overridden it returns the HashCode
        /// </summary>
        [JsonIgnore, NotMapped]
        public int CurrentCrc => GetCrc();
        /// <summary>
        /// calculates a hash code for a specified object, when the function is not overridden it retrieves the hashcode, which by design is not supposed to be constant.
        /// </summary>
        /// <returns></returns>
        public virtual int GetCrc() => GetHashCode();
        public HashedObject()
        {
            PrepareToSerialize += BaseObject_PrepareToSerialize;
        }
        ~HashedObject()
        {
            PrepareToSerialize -= BaseObject_PrepareToSerialize;
        }
        private void BaseObject_PrepareToSerialize(object? sender, EventArgs e)
        {
            SavedCrc = CurrentCrc;
            //throw new NotImplementedException();
        }
        public static void CallPrepareToSerialize()
        {
            PrepareToSerialize?.Invoke(null, EventArgs.Empty);
        }
        public virtual void UpdateItem()
        {
            // TODO: Implement UpdateItem on 12/22
            System.Diagnostics.Debug.WriteLine($"Update Item is not implemented in {GetType().Name}");
            using (Context.HintContext ctx = new Context.HintContext())
            {
                var needsAdding = !InDatabase;
                if (IsModified)
                {
                    if (InDatabase)
                    {
                        ctx.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        SetEntityStateOnDependentItems(ctx);
                        ctx.Update(this);
                        ctx.SaveChanges();
                        SetItemModifiedState(false);
                    }
                    else
                    {
                        ctx.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        ctx.Add(this);
                        ctx.SaveChanges(true);
                        SetItemModifiedState(false);
                    }
                }
            }
        }
        [NotMapped, JsonIgnore]
        protected virtual bool HasDependentCollections => false;

        protected Func<HashedObject, bool> Added = (hashedObject) => hashedObject.IsModified && !hashedObject.InDatabase;
        protected Func<HashedObject, bool> Modified = (hashedObject) => hashedObject.IsModified && hashedObject.InDatabase;
        protected virtual void SetEntityStateOnDependentItems(Context.HintContext context)
        {
            System.Diagnostics.Debug.WriteLine($"Link dependent items not implemented for {GetType().Name}, it is {(HasDependentCollections ? String.Empty : "not")} required.");
        }
        public virtual void SetItemModifiedState(bool state)
        {
            // TODO: Implement SetItemModifiedState on 12/22
            System.Diagnostics.Debug.WriteLine($"SetItemModifiedState not implemented in {GetType().Name}");
        }
    }
}