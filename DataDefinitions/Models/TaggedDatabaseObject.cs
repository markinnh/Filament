using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;

namespace DataDefinitions.Models
{
    public class TaggedDatabaseObject : DatabaseObject, ITag
    {
        private string _tags;
        public string Tags { get => _tags; set => Set(ref _tags, value); }

        public IEnumerable<string> GetTags()
        {
            if (!string.IsNullOrEmpty(Tags))
                return Tags.Split("#", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            else
                return null;
        }

    }
}
