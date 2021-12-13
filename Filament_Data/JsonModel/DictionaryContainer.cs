using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    public class DictionaryContainer<TKey, TValue> : Dictionary<TKey, TValue>, ILinked, IUpdateModified where TValue : ILinked
    {
        [JsonIgnore]
        public IDocument Document { get; protected set; }
        [JsonIgnore]
        public bool IsModified
        {
            get
            {

                return Values.IsModified();
            }
        }
        public DictionaryContainer()
        {
        }
        //public new ContainedType this[TKey index]
        //{
        //    get
        //    {
        //        if (TryGetValue(index, out ContainedType result))
        //            return result;
        //        else
        //            throw new KeyNotFoundException();

        //    }
        //}
        //public abstract IEnumerable<KeyValuePair<TKey, ContainedType>> Initializer();
        public new void Clear() { }

        public void EstablishLink(IDocument document)
        {
            Document = document;
            foreach (var item in Values)
            {
                item.EstablishLink(document);
            }
        }
        public TValue GetValue(TKey key)
        {
            if (TryGetValue(key, out TValue value))
                return value;
            else
                return default;
        }
        public void Add(TKey key, TValue value, bool establishLink)
        {
            Add(key, value);
            if (establishLink)
                value.EstablishLink(Document);
        }

        void IUpdateModified.SuccessfullySaved()
        {
            foreach (var item in Values)
            {
                if (item is IUpdateModified updateModified)
                    updateModified.SuccessfullySaved();
            }
        }
    }
}
