using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;
using DataDefinitions.Interfaces;
using LiteDB;

namespace DataDefinitions.JsonSupport
{
    public class DocumentLinkedCollection<T> : ObservableCollection<T>, ILinked, ITrackModified, IUpdateModified where T : ILinked
    {
        public bool InDataOperations => false;

        void IUpdateModified.SuccessfullySaved()
        {
            foreach (var item in this)
            {
                if (item is IUpdateModified updateModified)
                    updateModified.SuccessfullySaved();
            }
        }
        [JsonIgnore]
        public IJsonFilamentDocument Document { get; internal set; }
        [JsonIgnore]
        public virtual bool RequiresChildLinks => true;
        private bool isModified;
        [JsonIgnore, BsonIgnore]
        public bool IsModified
        {
            get =>isModified|| this.Count(i => i.IsModified) > 0; 
            set
            {
                isModified = value;
            }
        }
        public void EstablishLink(IJsonFilamentDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document), $"document is null, a valid reference is expected in {GetType().Name}.");
            foreach (var item in this)
            {
                //item.EstablishLink(document);
            }
        }
        public virtual void LinkChildren<ParentType>(ParentType parent)
        {
            //System.Diagnostics.Debug.Assert(RequiresChildLinks, $"{GetType().Name} requires the parent be linked to the child but it is not overriden, or the base method is called.");
            foreach (var item in this)
                item.LinkChildren(parent);
        }
        public new void Add(T item)
        {
            if (item != null)
            {
                base.Add(item);
                //item.EstablishLink(Document);
            }
            else
                throw new ArgumentNullException(nameof(item), $"trying to add a null item of type {typeof(T).Name} to the collection, this will lead to problems later");
        }
    }
}
