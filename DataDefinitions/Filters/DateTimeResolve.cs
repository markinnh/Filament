using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Filters
{
    public enum WhichCompare : short
    {
        Before = 0x10,
        After,
        Between
    };
    /// <summary>
    /// Provides a date filtering class
    /// </summary>
    /// <seealso cref="IDate"/>
    public class DateTimeResolve : IResolveFilter
    {
        public DateTime CompareDateTime { get; protected set; }
        public DateTime EndDateTime { get; protected set; }
        public WhichCompare Compare { get; protected set; }
        public DateTimeResolve()
        {
            Compare = WhichCompare.Before;
            CompareDateTime = DateTime.Today;
            EndDateTime = default(DateTime);
        }
        public DateTimeResolve(WhichCompare compare, DateTime compareDateTime, DateTime endDateTime = default)
        {
            Compare = compare;
            CompareDateTime = compareDateTime;
            EndDateTime = endDateTime;
        }
        public static bool operator ==(DateTimeResolve left, DateTimeResolve right)
        {
            return left.Compare == right.Compare && left.CompareDateTime == right.CompareDateTime && left.EndDateTime == right.EndDateTime;
        }
        public static bool operator !=(DateTimeResolve left, DateTimeResolve right)
        {
            return !(left == right);
        }
        public bool Accepted(object item)
        {
            bool result = true;
            if (item is IDate date)
                switch (Compare)
                {
                    case WhichCompare.Before:
                        result = date.DateTime <= CompareDateTime;
                        break;
                    case WhichCompare.After:
                        result = date.DateTime >= CompareDateTime;
                        break;
                    case WhichCompare.Between:
                        result = date.DateTime >= CompareDateTime && date.DateTime <= EndDateTime;
                        break;
                }
            return result;
        }
    }
}
