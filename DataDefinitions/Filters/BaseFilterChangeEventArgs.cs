using DataDefinitions.Interfaces;
using System;

namespace DataDefinitions.Filters
{
    public class BaseFilterChangedEventArgs<TPayload> : FilterChangedEventArgs
    {
        public TPayload Payload { get; set; }
        public BaseFilterChangedEventArgs(FilterAction actions,IResolveFilter.Filters filter,TPayload payload):base(actions,filter)
        {
            Payload = payload;
        }
    }
}
