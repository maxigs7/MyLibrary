using System.ComponentModel.DataAnnotations;
using Framework.Common.Services.Interfaces;

namespace Framework.Common.Services.Dtos
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        public virtual int SkipCount => (PageIndex-1) * PageSize;

        [Range(0, int.MaxValue)]
        public virtual int PageIndex { get; set; } = 1;
    }

    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int PageSize { get; set; } = 10;
    }
}