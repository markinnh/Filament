using DataDefinitions.LiteDBSupport;
using LiteDB;
using System.Dynamic;
using static System.Diagnostics.Debug;

namespace DataDefinitions;

public class ParentLinkedDataObject<ParentType> : DataObject where ParentType : DataObject
{
    [BsonIgnore]
    public ParentType Parent { get; set; }

    internal virtual void LinkToParent(ParentType parent)
    {
        Parent = parent;
    }
    public override void UpdateItem(LiteDBDal dal, bool holdContainerUpdate=false)
    {
        Assert(Parent != null, "Parent not initialized");
        PrepareToSave(dal);
        Parent.UpdateItem(dal);
    }
    internal override void DeleteMe(DataObject dataObject,ILiteDBDAL lite)
    {
        //Assert(this != dataObject, "Can't delete yourself.");
        Parent.DeleteMe(this,lite);
        dataObject = null;
    }
}
