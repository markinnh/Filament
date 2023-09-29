using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataDefinitions
{
    public class WordCollect : Observable
    {
        const string collectWords = @"(?<ignore>(\bthe\b)|(\bfor\b))|(?<word>\w{3,})";

        string[] baseIgnoreWords = new string[] { "the", "for" };

        private int minimumWordLength;

        public int MinimumWordLength
        {
            get => minimumWordLength;
            set => Set<int>(ref minimumWordLength, value);
        }

        private void buildCollectWords(string[] ignoreWords)
        {
            StringBuilder builder = new StringBuilder();
            // append the preamble
            builder.Append("(?<ignore>");
            foreach (string word in ignoreWords)
            {
                builder.Append(@$"(\b{word}\b)");
                builder.Append(word == ignoreWords.Last() ? ")|" : "|");
            }
            builder.Append(@$"(?<word>\w{{3,}})");
            CollectWords = builder.ToString();
        }
        public string CollectWords { get; set; } = collectWords;
        public void Initialize()
        {
            buildCollectWords(baseIgnoreWords);
        }
        public void Initialize(int minWordLength, string[] ignoreTheseWords = null)
        {
            MinimumWordLength = minWordLength;

            //if (ignoreTheseWords != null)
            //    ignoreWords = ignoreTheseWords;

            buildCollectWords(ignoreTheseWords == null ? baseIgnoreWords : ignoreTheseWords);
        }
        private IEnumerable<string> CollectTags(object item)
        {
            if (item != null && item is ITag tagItem && tagItem.GetTags() is IEnumerable<string> tags)
            {
                //if (tags != null)
                foreach (var tag in tags)
                    yield return tag;
            }
        }
        private IEnumerable<string> CollectKeywords(object item)
        {
            if (item is NoteDefn note)
            {
                if (Regex.IsMatch(note.Note, CollectWords) && Regex.Matches(note.Note, collectWords) is MatchCollection collection)
                {
                    foreach (Match match in collection)
                        if (!string.IsNullOrEmpty(match.Groups["word"].Value))
                            yield return match.Groups["word"].Value;
                }
            }
        }
        /// <summary>
        /// Should collect and collate keywords/tags by occurance within designated types
        /// </summary>
        /// <param name="objects">items to examine</param>
        /// <param name="handler">content retrieving method</param>
        /// <returns>An enumerable of a (string,int)</returns>
        private IEnumerable<WordWithOccuranceCount> OrganizeWords(IEnumerable objects, Func<object, IEnumerable<string>> handler)
        {
            List<string> words = new List<string>();
            foreach (var obj in objects)
            {
                words.AddRange(handler(obj));
            }
            var uniques = from word in words.Distinct()
                          select new WordWithOccuranceCount(word, words.Count(wd => wd == word));
            return uniques;
            //throw new NotImplementedException();
        }
        public IEnumerable<WordWithOccuranceCount> OrganizeTags(IEnumerable objects) { return OrganizeWords(objects, CollectTags); }
        public IEnumerable<WordWithOccuranceCount> OrganizeKeywords(IEnumerable objects) { return OrganizeWords(objects, CollectKeywords); }

        //public IEnumerable<WordWithOccuranceCount> OrganizeTags(IEnumerable objects, int minWordLength, string[] ignoreWords = null)
        //{
        //    Initialize(minWordLength, ignoreWords);
        //    return OrganizeTags(objects);
        //}
        public IEnumerable<WordWithOccuranceCount> OrganizeKeywords(IEnumerable objects, int minWordLength, string[] ignoreWords = null)
        {
            Initialize(minWordLength, ignoreWords);
            return OrganizeKeywords(objects);
        }
    }
}
