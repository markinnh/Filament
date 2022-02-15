using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;

namespace DataDefinitions.JsonSupport
{
    public class DocumentLinkedCollection<T> : ObservableCollection<T>, ILinked, ITrackModified, IUpdateModified where T : ILinked
    {
        void IUpdateModified.SuccessfullySaved()
        {
            foreach (var item in this)
            {
                if (item is IUpdateModified updateModified)
                    updateModified.SuccessfullySaved();
            }
        }
        [JsonIgnore]
        public IDocument Document { get; internal set; }
        [JsonIgnore]
        public bool IsModified { get => this.Count(i => i.IsModified) > 0; }
        public void EstablishLink(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document), $"document is null, a valid reference is expected in {GetType().Name}.");
            foreach (var item in this)
            {
                item.EstablishLink(document);
            }
        }
        public new void Add(T item)
        {
            if (item != null)
            {
                base.Add(item);
                item.EstablishLink(Document);
            }
            else
                throw new ArgumentNullException(nameof(item), $"trying to add a null item of type {typeof(T).Name} to the collection, this will lead to problems later");
        }
    }
}
