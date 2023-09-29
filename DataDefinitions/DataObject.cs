using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using LiteDB;
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace DataDefinitions
{
    /// <summary>
    /// dependent objects in a Document Database, will not require a 'key id' but can have dependent objects
    /// </summary>
    public class DataObject : Observable, ITrackModified
    {
        [BsonIgnore]
        public virtual bool InDataOperations => false;

        private bool isModified;
        [BsonIgnore, Affected(Names = new string[] { nameof(IsValid) })]
        public virtual bool IsModified { get => isModified; set => Set(ref isModified, value); }
        [BsonIgnore]
        public virtual bool IsValid => false;
        [BsonIgnore]
        public virtual bool CanEdit => !InDataOperations;
        //[BsonIgnore]
        protected virtual bool HasContainedItems => false;
        [BsonIgnore]
        public virtual bool SupportsDelete => this is ISupportDelete;

        public virtual void SetContainedModifiedState(bool value) { }
        public virtual string UIHintAddType() => GetType().GetCustomAttribute<UIHintsAttribute>()?.AddType ?? string.Empty;
        public virtual void PostDataRetrieveActions()
        {

        }
        //protected override bool Set<T>(ref T target, T value, bool blockUpdate = false, [CallerMemberName] string propertyName = null)
        //{
        //    if (base.Set(ref target, value, blockUpdate, propertyName) && !InDataOperations && propertyName != nameof(IsModified) && !blockUpdate)
        //    {
        //        IsModified = true;
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// This method is overidden in the DatabaseObject and ParentLinkedDataObject.
        /// </summary>
        /// <param name="dal">the dataaccesslayer</param>
        public virtual void UpdateItem(LiteDBDal dal)
        {
            //Assert(false, "This method should not be called, the dataobject is a base object");
        }
        /// <summary>
        /// Only supported in ParentLinkedDataObject
        /// </summary>
        /// <param name="dataObject">Object to be 'deleted'</param>
        internal virtual void DeleteMe(DataObject dataObject) { }
        internal virtual void PrepareToSave(LiteDBDal dBDal)
        {

        }
    }
}
