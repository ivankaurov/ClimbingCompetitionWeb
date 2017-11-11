using System.Collections.Generic;

namespace Climbing.Web.Utilities
{
    public interface IPagedCollection<T> : IEnumerable<T>
    {
        ICollection<T> Page { get; }

        int PageNumber { get; }

        int TotalPages { get; }

        int PageSize { get; }
    }
}