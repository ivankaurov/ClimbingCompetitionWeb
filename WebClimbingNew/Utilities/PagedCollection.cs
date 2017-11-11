using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Climbing.Web.Utilities
{
    public class PagedCollection<T> : IPagedCollection<T>
    {
        public PagedCollection(IEnumerable<T> page, int pageNumber, int totalPages, int pageSize)
        {
            Guard.NotNull(page, nameof(page));
            Guard.Requires(pageNumber >= 0, nameof(pageNumber), nameof(pageNumber) + " should be non-negative");
            Guard.Requires(totalPages >= 0, nameof(totalPages), nameof(totalPages) + " should be non-negative");
            Guard.Requires(pageSize > 0, nameof(pageSize), nameof(pageSize) + " should be positive");

            this.Page = page.AsCollection();
            this.PageNumber = pageNumber;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
        }

        public ICollection<T> Page { get; }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public static PagedCollection<T> Empty(int pageSize) => new PagedCollection<T>(new T[0], 0, 0, pageSize);

        public IEnumerator<T> GetEnumerator() => this.Page.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}