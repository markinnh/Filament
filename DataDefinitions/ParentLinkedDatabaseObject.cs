using DataDefinitions.LiteDBSupport;
using LiteDB;
using static System.Diagnostics.Debug;

namespace DataDefinitions;

public class ParentLinkedDatabaseObject<ParentType> : DatabaseObject where ParentType : DataObject
{
    [BsonIgnore]
    public ParentType Parent
    {
        get; set;
    }
    internal virtual void LinkToParent(ParentType parent)
    {
        Assert(parent != null);

        Parent = parent;
    }
    public override void UpdateItem(LiteDBDal dal, bool holdContainerUpdate = false)
    {
        /// <remarks>
        /// if the ParentLinkedDatabaseObject is not in the database, the parent will need to be updated too
        /// since the Parent object usually has a reference id from the target item, also, this reference id needs to be established before 
        /// the parent Object is sent to the database
        /// </remarks>

        base.UpdateItem(dal, holdContainerUpdate);

        if (!holdContainerUpdate)
        {
            Parent.UpdateItem(dal,holdContainerUpdate);

            IsModified = false;
        }
    }
    internal override void DeleteMe(DataObject dataObject, ILiteDBDAL lite)
    {
    }
}
