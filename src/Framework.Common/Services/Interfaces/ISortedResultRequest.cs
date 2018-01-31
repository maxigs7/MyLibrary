using Framework.Common.Entities.Enum;

namespace Framework.Common.Services.Interfaces
{
    /// <summary>
    /// This interface is defined to standardize to request a sorted result.
    /// </summary>
    public interface ISortedResultRequest
    {
        /// <summary>
        /// Sorting information.
        /// Should include sorting field and optionally a direction (ASC or DESC)
        /// Can contain more than one field separated by comma (,).
        /// </summary>
        /// <example>
        /// Examples:
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </example>
        string Sorting { get; set; }

        /// <summary>
        /// Sorting information.
        /// Should include sorting field
        /// </summary>
        string SortBy { get; set; }

        /// <summary>
        /// Sorting information.
        /// Should include sorting direction (ASC or DESC)
        /// </summary>
        SortDirectionEnum? SortDirection { get; set; }

        string SortingCalculate { get; }
    }
}