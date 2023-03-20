using System.Collections.Generic;

namespace Infra
{
    public class PaginationDto<T>
    {
        public ICollection<T> Data { get; set; } = new HashSet<T>();
        public int PageCount { get; set; }
    }
}
