using Framework.Common.Entities.Enum;
using Framework.Common.Extensions;
using Framework.Common.Services.Interfaces;

namespace Framework.Common.Services.Dtos
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public string Sorting { get; set; }
        public string SortBy { get; set; }
        public SortDirectionEnum? SortDirection { get; set; }

        public virtual string SortingCalculate
        {
            get
            {
                if (SortBy.IsNullOrWhiteSpace()) return null;
                return SortBy + " " + (SortDirection ?? SortDirectionEnum.Asc);
            }
        }
    }
}