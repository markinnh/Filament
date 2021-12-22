using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace HintLib
{
    public class Hint : HashedObject
    {
        const string initialContent = @"<FlowDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">

   <Paragraph>
This is a starting hint.  You can <Span Foreground=""Red"">edit</Span> it as you see fit.
</Paragraph>
</FlowDocument>";
        public int HintId { get; set; }
        [JsonIgnore,NotMapped]
        public override bool InDatabase => HintId!=default;

        private string tags = String.Empty;

        public string Tags
        {
            get => tags;
            set => Set<string>(ref tags, value);
        }

        private string title = String.Empty;

        public string Title
        {
            get => title;
            set => Set<string>(ref title, value);
        }

        public override int GetCrc()
        {
            if (content != null)
            {
                Crc16 crc16 = new Crc16();
                var result = crc16.ComputeChecksum(Encoding.Unicode.GetBytes(content));
                return result; //tags.GetHashCode() ^ title.GetHashCode() ^ content.GetHashCode();
            }
            else
                return base.GetHashCode();
        }

        private string content = initialContent;
        /// <summary>
        /// The actual 'formatted content' that is displayed.
        /// </summary>
        public string Content
        {
            get => content;
            set => Set<string>(ref content, value);
        }
        [JsonIgnore]
        public int HintDataId { get; set; }
        [JsonIgnore]
        public virtual HintProject? HintData { get; set; }

        [JsonIgnore]
        public string DisplayContent { get => content; }

        public override void SetItemModifiedState(bool state)
        {
            IsModified = state;
        }
        public Hint()
        {
            SavedCrc = GetCrc();
        }

        #region NamedColorProvider
        [JsonIgnore,NotMapped]
        public static IOrderedEnumerable<ColorUsageTracker> Colors { get; } = GetTrackers();
        
        private static IOrderedEnumerable<ColorUsageTracker> GetTrackers()
        {

            string[] primaryColors = { "Black", "White", "Red", "Green", "Yellow", "Blue",
                "Gray", "Olive", "Maroon", "Purple","Silver","Fuschia","Lime","Navy","Teal","Aqua" };
            var drawingColorNames = from string c in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public).Select(c => c.Name)
                                    select new ColorUsageTracker() { Color = c, UsageCount = primaryColors.Contains(c) ? 5 : 0 };

            var trackers = from ColorUsageTracker c in drawingColorNames
                           orderby c.UsageCount descending
                           select c;
            return trackers;
        }
        #endregion

    }
}
