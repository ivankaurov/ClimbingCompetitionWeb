namespace Climbing.Web.Utilities
{
    using System.Collections.Generic;

    public interface IPagedCollection<T> : IEnumerable<T>
    {
        ICollection<T> Page { get; }

        int PageNumber { get; }

        int TotalPages { get; }

        int PageSize { get; }
    }
}