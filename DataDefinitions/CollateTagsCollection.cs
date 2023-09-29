using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions
{
    public class CollateTagsCollection<TColl> : ObservableCollection<TColl>, ITagCollate where TColl : ITag
    {
        public Guid Signature { get;protected set; } = Guid.NewGuid();
        public CollateTagsCollection(IEnumerable<TColl> colls) : base(colls) { }
        private IEnumerable<string> LocalTags()
        {
            
            foreach (var item in this)
                if (item.GetTags() is IEnumerable<string> tags)
                    foreach (var t in tags)
                        yield return t;
        }
        public IEnumerable<WordWithOccuranceCount> DistinctTagStats
        {
            get
            {
                //var tags = LocalTags().ToArray();
                //var result = from string t in tags
                //             select new TagStat(t, tags.Count(s => s == t));
                return Singleton<WordCollect>.Instance.
                    OrganizeTags(this).
                    DistinctBy(t => t.Word).
                    OrderByDescending(t=>t.OccuranceCount);
            }
            //throw new NotImplementedException();
        }
    }
}
