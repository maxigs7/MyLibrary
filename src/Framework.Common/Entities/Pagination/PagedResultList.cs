using System.Linq;

namespace Framework.Common.Entities.Pagination
{
    public class PagedResultList<T>
    {
        public PagedMetadata PagedMetadata { get; set; }

        public IQueryable<T> Entities { get; set; }
    }
}
