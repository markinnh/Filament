using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public class ShowAllResolve : ICriteriaFilter
    {
        public Guid Signature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CriteriaSet => true;
        //private bool _applied;
        //public bool Applied { get => _applied; set => _applied=value; }

        public bool Accepted(object item)
        {
            if (item is ITrackUsable usable)
                return showAll ? true : !usable.StopUsing ?? false;
            else
                return true;
            //throw new NotImplementedException();
        }
        private bool showAll;
        public void SetCriteria(object criteria)
        {
            if (criteria is bool show)
                showAll = show;
            else
                throw new NotSupportedException($"{criteria.GetType().Name} is not supported in {GetType().Name}");
        }

        public void UpdateCriteria(object criteria)
        {
            SetCriteria(criteria);
            //throw new NotImplementedException();
        }
    }
}
