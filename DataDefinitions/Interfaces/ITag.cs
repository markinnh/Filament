using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    public interface ITag
    {
        string Tags { get; set; }
        IEnumerable<string> GetTags();
    }
    public interface ITagCollate
    {
        /// <summary>
        /// Returns an enumerable of distinct tags in a collection
        /// </summary>
        /// <returns>Distinct tag collection</returns>
        public IEnumerable<TagStat> DistinctTagStats { get; }
        Guid Signature { get; }
    }
}
