using DataDefinitions.LiteDBSupport;
using LiteDB;
using System.Dynamic;
using static System.Diagnostics.Debug;

namespace DataDefinitions
{
    public class ParentLinkedDataObject<ParentType> : DataObject where ParentType : DataObject
    {
        [BsonIgnore]
        public ParentType Parent { get; set; }

        internal virtual void LinkToParent(ParentType parent)
        {
            Parent = parent;
        }
        public override void UpdateItem(LiteDBDal dal)
        {
            Assert(Parent != null, "Parent not initialized");
            PrepareToSave(dal);
            Parent.UpdateItem(dal);
        }
        internal override void DeleteMe(DataObject dataObject)
        {
            Assert(this != dataObject, "Can't delete yourself.");
            dataObject=null;
        }
    }
}
