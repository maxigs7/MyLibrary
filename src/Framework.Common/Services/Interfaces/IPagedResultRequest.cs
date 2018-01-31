namespace Framework.Common.Services.Interfaces
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// Skip count (beginning of the page).
        /// </summary>
        int SkipCount { get; }

        int PageIndex { get; set; }
    }
}