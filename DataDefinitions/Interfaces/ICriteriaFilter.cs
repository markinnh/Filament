using System;

namespace DataDefinitions.Interfaces
{
    public interface ICriteriaFilter : IResolveFilter
    {
        Guid Signature { get; set; }
        /// <summary>
        /// Flag to tell if the criteria is set, looks bad
        /// </summary>
        bool CriteriaSet { get; }
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
