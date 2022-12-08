using DataDefinitions;
using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    internal class TagFilter : Observable, IKeywordFilter
    {
#if DEBUG
        public object Owner { get; set; }
#endif
        public Guid Signature { get; set; }

        IEnumerable<string>? _tags;
        private bool _applied;
        public bool Applied { get => _applied; set => Set(ref _applied, value); }
        public IEnumerable<string> Keywords { get=>_tags; set=>Set(ref _tags,value); }
        public bool CriteriaSet => _tags != null;
        public void Filter(object sender, FilterEventArgs e)
        {
            if (_tags != null)
            {
                switch (e.Item)
                {
                    case VendorDefn defn:
                        //e.Accepted = _tags.All(t => defn.Tags?.Contains($"#{t} ") ?? false);
                        e.Accepted = _tags.All(t => Regex.IsMatch(defn.Tags ?? string.Empty, @$"#{t}\b"));
                        break;
                    case InventorySpool spool:
                        //e.Accepted = _tags.All(t => spool.Parent.Parent.Tags?.Contains($"#{t} ") ?? false);
                        e.Accepted = _tags.All(t => Regex.IsMatch(spool.Parent.Parent.Tags ?? string.Empty, @$"#{t}\b"));
                        break;
                    case SpoolDefn spoolDefn:
                        e.Accepted = _tags.All(t => Regex.IsMatch(spoolDefn.Parent.Tags ?? string.Empty, @$"#{t}\b"));
                        break;
                    case NoteDefn noteDefn:
                        e.Accepted = _tags.All(t => Regex.IsMatch(noteDefn.Tags ?? string.Empty, @$"#{t}\b"));
                        break;
                    default:
                        throw new NotSupportedException($"{e.Item.GetType().Name} is not supported in the TagFilter");
                }
            }
            else
                e.Accepted = true;
        }

    }
}
