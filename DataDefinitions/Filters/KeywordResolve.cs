using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class KeywordResolve : ICriteriaFilter
    {
        public Guid Signature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CriteriaSet => !string.IsNullOrEmpty(_regexKeywordFilter);

        private string _regexKeywordFilter;
        private IEnumerable<string> _keywords;
        public bool Accepted(object item)
        {
            bool result = true;
            if (CriteriaSet)
            {
                if (item is NoteDefn note)
                {
                    result= Regex.IsMatch(note.Note, _regexKeywordFilter);
                }
            }
            
            return result;
            //throw new NotImplementedException();
        }

        public void SetCriteria(object criteria)
        {
            if(criteria is KeywordFilterChangedEventArgs keyword)
            {
                _keywords = keyword.Payload;
                BuildRegexFilter(_keywords);
            }
            //throw new NotImplementedException();
        }
        private void BuildRegexFilter(IEnumerable<string> keywords)
        {
            int lastPos = _keywords.Count();
            int curPos = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var keyword in _keywords)
            {
                sb.Append($"({keyword})");
                curPos++;
                if (curPos < lastPos)
                    sb.Append('|');
            }
            _regexKeywordFilter = sb.ToString();
        }
        public void UpdateCriteria(object criteria)
        {
            SetCriteria(criteria);
            //throw new NotImplementedException();
        }
    }
}
