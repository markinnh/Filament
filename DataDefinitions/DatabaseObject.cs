using DataDefinitions.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;
using System.Reflection;
using MyLibraryStandard.Attributes;
using System.Linq.Expressions;
using System.Xml.Serialization;
using DataDefinitions.Interfaces;
using LiteDB;
using DataDefinitions.LiteDBSupport;

namespace DataDefinitions
{
    public enum DatabaseObjectActions { ItemAdded, ItemRemoved };

    /// <summary>
    /// Root items that are stored in the LiteDb database, not called documents because they don't support the LiteDb Document class
    /// </summary>
    [UIHints(AddType = "Not supported")]
    public class DatabaseObject : DataObject,ILinked
    {
        protected const string keyInitializedError = "Key ID is already initialized and AssignKey is called";

        /// <summary>
        /// Sets a Property value through the backing member
        /// </summary>
        /// <returns>true if the property was changed</returns>
        protected static int passUpCount = 0;
        /// <summary>
        /// Flag to tell if data operations are currently being conducted
        /// </summary>
        /// <remarks>prevents editing of objects while this is going on</remarks>
        /// <value>whether or not database operations are currently being conducted</value>
        //[JsonIgnore, BsonIgnore]
        //public virtual bool InDataOperations { get; }

        /// <summary>
        /// Tracks the ability to modify contents of an object through the UI
        /// </summary>
        /// <remarks>Overridden in FilamentDefn to prevent editing items that are flagged as 'IsIntrisic'</remarks>
        /// <value>Usually true</value>
        //[JsonIgnore, BsonIgnore]
        //public virtual bool CanEdit => !InDataOperations;
        /// <summary>
        /// flag to tell if the current object is 'ready' to be saved to the database
        /// </summary>
        /// <value>true or false</value>
        //[JsonIgnore, BsonIgnore]
        //public virtual bool IsValid { get => false; }
        //protected bool isModified;
        ///// <summary>
        ///// flag to tell if the object is modified
        ///// </summary>
        ///// <value>state of object compared to the database</value>
        //[JsonIgnore, Affected(Names = new string[] { nameof(InDatabase) }),XmlIgnore,BsonIgnore]
        //public virtual bool IsModified
        //{
        //    get => isModified;
        //    set => Set<bool>(ref isModified, value);
        //}
        /// <summary>
        /// Checks if the PrimaryKey is a non-default value
        /// </summary>
        /// <value>whether or not the item is in the database</value>
        /// <remarks><b>needs to be rewritten to support items that do not have a 'key', or all items that are 'serialized' need a key, which would be a simpler solution</b></remarks>
        [JsonIgnore, BsonIgnore]
        public virtual bool InDatabase { get => KeyID != default; }
        /// <summary>
        /// needed for XAML markup, unless using a NOT converter
        /// </summary>
        [JsonIgnore,BsonIgnore]
        public bool NotInDatabase => !InDatabase;

        /// <summary>
        /// Supports delete, default is false, each definition will need a decision made about supporting a delete.
        /// </summary>
        //[JsonIgnore, BsonIgnore]
        //public virtual bool SupportsDelete => false;
        ///// <summary>
        ///// An internal tracking feature to monitor which class have not implemented SetContainedModifiedState
        ///// </summary>
        ///// <remarks>Should be overridden if this diagnostic is needed in troubleshoot, but currently the classes that have contained items implement SetContainedModifiedState</remarks>
        ///// <value>intially set to false</value>
        //[JsonIgnore,BsonIgnore]
        //protected virtual bool HasContainedItems => false;
        /// <summary>
        /// JsonDocument reference
        /// </summary>
        /// <remarks>Contains a reference to a JsonDocument</remarks>
        /// <value>JsonDocument</value>
        //[JsonIgnore,XmlIgnore,BsonIgnore]
        //public IJsonFilamentDocument Document { get; protected set; }
        [JsonIgnore,BsonIgnore]
        public virtual bool RequiresChildLinks => true;

        private bool inEdit;
        /// <summary>
        /// flag to tell if the item is being edited in a UWP DataGrid
        /// </summary>
        /// <value>true or false</value>
        [JsonIgnore,XmlIgnore,BsonIgnore]
        public bool InEdit
        {
            get => inEdit;
            set => Set<bool>(ref inEdit, value);
        }
        /// <summary>
        /// Called to 'Update' an object to the database.
        /// </summary>
        /// <remarks>Has enough logic to 'flag' the contained items for saving too.  does need to be virtual,
        /// since there is one class that overrides the 'default' save routine.  Changed for JSON DatabaseObjects</remarks>
        public virtual void UpdateItem()
        {
            WriteLine($"UpdateItem using base definition for {GetType().Name}");
            // TODO: Rewrite UpdateItem to support JSONDocuments

            // call AssignKey, the logic is contained within the overriden methods
            // AssignKey();
            // if(!
            //using (TContext context = new TContext())
            //{
            //    SetDataOpsState(true);
            //    if (InDatabase)
            //    {
            //        context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //        UpdateContainedItemEntryState(context);
            //        context.Update(this);
            //        context.SaveChanges();
            //    }
            //    else
            //    {
            //        context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            //        UpdateContainedItemEntryState(context);
            //        context.Add(this);
            //        context.SaveChanges();
            //        //OnPropertyChanged(nameof(InDatabase));
            //    }
            //    SetDataOpsState(false);

            //    SetContainedModifiedState(false);
            //    //IsModified = false;

            //}
            var needsAdd = !InDatabase;
            //if (!InDatabase)
            //    AssignKey(Document.Counters.NextID(this));

            UpdateContainedItems();
            
            if (needsAdd)
            {
                //TODO: Develop a virtual method to save the object to the 'database' should only apply for Settings, VendorDefn, FilamentDefn and PrintSettingDefn
                SaveToJsonDatabase();
            }
        }
        /// <summary>
        /// This is the only method that actually 'stores' data into the database, all the others walk up the tree until they reach the 'root' DatabaseObject
        /// </summary>
        /// <param name="dal"></param>
        public override void UpdateItem(LiteDBDal dal)
        {
            dal.SmartAdd(this);
            //if (InDatabase)
            //    dal.Update(this);
            //else
            //    dal.Add(this);
        }
        protected virtual void SaveToJsonDatabase()
        {
            // does nothing for most methods, but does actually save item in the case of Setting, VendorDefn, FilamentDefn and PrintSettingDefn
        }
        [JsonIgnore, BsonIgnore]
        protected virtual bool NeedsKey => false;
        internal virtual void AssignKey(int myId)
        {
            if (KeyID == default)
                KeyID = myId;
            else
                ReportKeyAlreadyInitialized();

        }
        [JsonIgnore]
        internal virtual int KeyID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <summary>
        /// Establishes a JsonDocument link
        /// </summary>
        /// <remarks>Currently not finished</remarks>
        //public virtual void EstablishLink(IJsonFilamentDocument document)
        //{
        //    if (document != null)
        //    {
        //        Document = document;
        //        // TODO: Add code to link dependent objects
        //        LinkChildren(this);
        //    }
        //    else
        //        ReportNullDocument(nameof(document));
        //}
        //protected void ReportNullDocument(string documentName)
        //{
        //    throw new ArgumentNullException(documentName, $"{documentName} is null, a non-null value is required");
        //}
        protected void ReportKeyAlreadyInitialized() => throw new ArgumentException(keyInitializedError);
        public virtual void LinkChildren<ParentType>(ParentType parent)
        {
            Assert(RequiresChildLinks, $"{GetType().Name} requires childlinks, but this method is not overridden, or calls the base class.");
        }
        /* not used for JSONDocuments
        /// <summary>
        /// Called during Save/Update to database
        /// </summary>
        /// <remarks>Allows changing contained items state to either Added or Modified.</remarks>
        internal virtual void UpdateContainedItemEntryState<TContext>(TContext context) where TContext : DbContext
        {
            WriteLine($"{nameof(UpdateContainedItemEntryState)} not implemented for {GetType().Name}");
        }*/
        /// <summary>
        /// Set the Database Operations state
        /// </summary>
        /// <remarks>Currently supports a boolean, either InDatabaseOperations or not InDatabaseOperations</remarks>
        protected virtual void SetDataOpsState(bool state)
        {
            WriteLine($"In the base {nameof(SetDataOpsState)}, this should be implemented by each consumer of DatabaseObject");
        }
        internal virtual void UpdateContainedItems()
        {

        }
        /// <summary>
        /// Set the contained items IsModified 'state'
        /// </summary>
        /// <remarks>Usually called after saving the item to the database</remarks>
        //public virtual void SetContainedModifiedState(bool state)
        //{
        //    WriteLine($"Unable to set the ContainedModifiedState to '{state}' for {GetType().Name}, this is{(HasContainedItems ? string.Empty : " not")} required.");
        //    Assert(!HasContainedItems);  // This should only fire if this is not implemented and the class has contained items.
        //}

        public virtual void AddChild() => WriteLine($"Add child not supported in {GetType().Name}");
        /// <summary>
        /// Probably Obsolete
        /// </summary>
        //protected bool AddedItems<TItem>(TItem item) where TItem : DepthMeasurement
        //{
        //    if (item != null)
        //        return !item.InDatabase && item.IsValid;
        //    else
        //        return false;
        //}
        /// <summary>
        /// Tests if a DatabaseObject was just created but not currently in the database
        /// </summary>
        protected Func<DatabaseObject, bool> Added = (databaseObject) => !databaseObject.InDatabase && databaseObject.IsValid;
        /// <summary>
        /// Tests if a DatabaseObject is in the database, but modified
        /// </summary>
        protected Func<DatabaseObject, bool> Modified = (databaseObject) => databaseObject.IsValid && databaseObject.IsModified && databaseObject.InDatabase;
        /* not used for JSONDocuments
        /// <summary>
        /// Removes the referenced DatabaseObject from the database
        /// </summary>
        /// <remarks>Currently only supported for InventorySpool and DepthMeasurement</remarks>
        public virtual void Delete<TContext>() where TContext : DbContext, new()
        {
            using (TContext context = new TContext())
            {
                context.Remove(this);
                context.SaveChanges();
            }
        }*/
        /// <summary>
        /// Handle notifications of Properties being updated in contained members
        /// </summary>
        /// <remarks>To allow monitoring of the 'IsModified' and reflecting it to the containing class.</remarks>
        protected override void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
        {
            base.WatchContainedHandler(sender, e);
            switch (e.PropertyName)
            {
                case nameof(IsModified):

                    if (passUpCount == 0 || (passUpCount > 0 && GetType() != typeof(VendorDefn) || GetType() != typeof(FilamentDefn)))
                    {
                        OnPropertyChanged(nameof(IsModified));
                        passUpCount++;
                    }
                    // don't call the event handler again if this is the second time through the method
                    else if ((GetType() == typeof(VendorDefn) && GetType() == typeof(FilamentDefn) && passUpCount != 0))
                        passUpCount = 0;
                    else
                        passUpCount = 0;
                    break;
            }
        }
    }
}
