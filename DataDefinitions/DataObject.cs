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
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace DataDefinitions;

/// <summary>
/// dependent objects in a LiteDb document database, will not require a 'key id' but can have dependent objects
/// </summary>
public class DataObject : Observable, ITrackModified
{
    [BsonIgnore, JsonIgnore]
    public virtual bool InDataOperations => false;

    private bool isModified;
    [BsonIgnore, Affected(Names = new string[] { nameof(IsValid) }),JsonIgnore]
    public virtual bool IsModified { get => isModified; set => Set(ref isModified, value); }
    [BsonIgnore,JsonIgnore]
    public virtual bool IsValid => false;
    [BsonIgnore,JsonIgnore]
    public virtual bool CanEdit => !InDataOperations;

#if DEBUG
    [BsonIgnore,JsonIgnore]
    public bool IsDebug => true;
#else
    [BsonIgnore, JsonIgnore]
    public bool IsDebug => false;
#endif

    /// <summary>
    /// A flag to determine if an object has any items in an internal container
    /// </summary>
    [BsonIgnore, JsonIgnore]
    protected virtual bool HasContainedItems => false;
    [BsonIgnore, JsonIgnore]
    public virtual bool SupportsDelete => this is IDeleteSupport;

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
    public virtual void UpdateItem(LiteDBDal dal,bool holdContainerUpdate=false)
    {
        //Assert(false, "This method should not be called, the dataobject is a base object");
    }
#if DEBUG
    /// <summary>
    /// Generate code for a specific dataobject to be sent to a location for retrieval
    /// </summary>
    /// <returns>code generated</returns>
    public virtual string GenerateCode(int tabCount=1,string ownerName="")
    {
        Assert(false);
        return string.Empty;
    }
    protected string DoQuote(string target) => $"\u0022{target}\u0022";
#endif
    /// <summary>
    /// Only supported in ParentLinkedDataObject
    /// </summary>
    /// <param name="dataObject">Object to be 'deleted'</param>
    internal virtual void DeleteMe(DataObject dataObject,ILiteDBDAL lite) { }
    internal virtual void PrepareToSave(LiteDBDal dBDal)
    {

    }
}
