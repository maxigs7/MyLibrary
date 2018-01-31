using Framework.Common.Entities.Interfaces;

namespace Framework.Data
{
    /// <summary>
    /// Standard filters.
    /// </summary>
    public static class DataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="ISoftDelete"/> interface.
        /// </summary>
        public const string SoftDelete = "SoftDelete";

    }
}
