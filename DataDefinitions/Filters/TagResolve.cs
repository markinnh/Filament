using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class TagResolve : ICriteriaFilter
    {
        private Guid _signature;
        public Guid Signature { get => _signature; set => _signature = value; }
        IEnumerable<string> _tags;
        public bool CriteriaSet => _tags != null;
        //private bool _applied;
        //public bool Applied { get => _applied; set => _applied = value; }

        public bool Accepted(object item)
        {
            bool result = true;
            if (CriteriaSet)
            {
                var tagContent = TagContent(item);

                result = !string.IsNullOrEmpty(tagContent) ? _tags.All(t=>Regex.IsMatch(tagContent, @$"#{t}\b")) : false;

                //switch (item)
                //{
                //    case ITag defn:
                //        //e.Accepted = _tags.All(t => defn.Tags?.Contains($"#{t} ") ?? false);
                //        result = _tags.All(t => Regex.IsMatch(defn.Tags ?? string.Empty, @$"#{t}\b"));
                //        break;
                //    case InventorySpool spool:
                //        //e.Accepted = _tags.All(t => spool.Parent.Parent.Tags?.Contains($"#{t} ") ?? false);
                //        result = _tags.All(t => Regex.IsMatch(spool.Parent?.Parent?.Tags ?? string.Empty, @$"#{t}\b"));
                //        break;
                //    case SpoolDefn spoolDefn:
                //        result = _tags.All(t => Regex.IsMatch(spoolDefn.Parent?.Tags ?? string.Empty, @$"#{t}\b"));
                //        break;
                //    default:
                //        throw new NotSupportedException($"{item.GetType().Name} is not supported in the TagFilter");
                //}
            }
            return result;
            //throw new NotImplementedException();
        }
        private static string GetTagContent(object item)
        {
            string result = string.Empty;
            switch (item)
            {
                case ITag tag:
                    result = tag.Tags;
                    break;
                case InventorySpool spool:
                    try
                    {
                        result = spool?.Parent?.Parent?.Tags;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Error accessing the parent tag for the inventory spool");
                    }
                    break;
                case SpoolDefn spool:
                    try
                    {
                        result = spool.Parent?.Tags ?? string.Empty;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Error accessing the parent tag for the spool definition");
                    }
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
        private static string TagContent(object item) => item switch
        {
            ITag tag => tag.Tags,
            InventorySpool spool => spool?.Parent?.Parent?.Tags ?? string.Empty,
            SpoolDefn spoolDefn => spoolDefn?.Parent?.Tags ?? string.Empty,
            _ => string.Empty
        };
        public void SetCriteria(object criteria)
        {
            if (criteria is TagFilterChangedEventArgs tagFilter)
            {
                Signature = tagFilter.TagGuid;
                _tags = tagFilter.Payload;
            }
            //throw new NotImplementedException();
        }

        public void UpdateCriteria(object criteria)
        {
            if (criteria is TagFilterChangedEventArgs tagFilter && tagFilter.TagGuid == Signature)
            {
                if (tagFilter.TagGuid == Signature)
                {
                    _tags = tagFilter.Payload;
                }
            }
            //throw new NotImplementedException();
        }
    }
}
