using System;
using System.Collections.Generic;
using System.Text;

namespace DataDefinitions.JsonSupport
{
    public interface ITrackModified
    {
        bool IsModified { get; }
    }
    public class CounterProvider:Dictionary<string,int>,ITrackModified
    {
        const int startIndex = 1;
        public bool IsModified { get;internal set; }
        public void SuccessfullySaved()
        {
            IsModified = false;
        }
        public int NextID(Type type,int startingID = startIndex)
        {
            if (ContainsKey(type.FullName))
            {
                //++this[type.FullName];
                //this[type.FullName] = result;
                IsModified = true;
                return ++this[type.FullName];
            }
            else
            {

                Add(type.FullName, startingID);
                IsModified = true;
                return this[type.FullName];
            }
        }
    }
}
