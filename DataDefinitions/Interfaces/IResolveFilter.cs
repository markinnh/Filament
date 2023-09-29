using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    /// <summary>
    /// actually 'resolves' or provides the criteria for each filter, which changes based on the type of data being examined
    /// </summary>
    public interface IResolveFilter
    {
        public enum Filters : short
        {
            ShowAll,
            Tag,
            Keyword,
            Date
        }
        ///// <summary>
        ///// Determines if the Filter is applied
        ///// </summary>
        //bool Applied { get; set; }
        /// <summary>
        /// Checks if the item is accepted
        /// </summary>
        /// <param name="item">item to examine</param>
        /// <returns>criteria satified</returns>
        bool Accepted(object item);
    }
}
