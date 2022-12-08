using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    public interface IFilterOld
    {
        Guid Signature { get; set; }
        /// <summary>
        /// Determines if the Filter is applied
        /// </summary>
        bool Applied { get; set; }
        /// <summary>
        /// Flag to tell if the criteria is set, looks bad
        /// </summary>
        bool CriteriaSet { get; }
        /// <summary>
        /// Checks if the item is accepted
        /// </summary>
        /// <param name="item">item to examine</param>
        /// <returns>criteria satified</returns>
        bool Accepted(object item);
        /// <summary>
        /// Initialize the filter criteria
        /// </summary>
        /// <param name="criteria">criteria to be initialized with</param>
        void SetCriteria(object criteria);
        /// <summary>
        /// Update the filter criteria
        /// </summary>
        /// <param name="criteria">updated criteria</param>
        void UpdateCriteria(object criteria);
    }
}
