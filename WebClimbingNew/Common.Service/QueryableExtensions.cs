namespace Climbing.Web.Common.Service
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service.Facade;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;

    internal static class QueryableExtensions
    {
        public static async Task<IPagedCollection<T>> ApplyPaging<T>(this IQueryable<T> query, IPageParameters paging, CancellationToken cancellationToken = default(CancellationToken))
        {
            var skip = (paging.PageNumber - 1) * paging.PageSize;

            var count = await query.CountAsync(cancellationToken);

            if (count == 0)
            {
                return PagedCollection<T>.Empty(paging.PageSize);
            }

            var totalPages = count / paging.PageSize;
            if ((count % paging.PageSize) > 0)
            {
                totalPages++;
            }

            if (skip >= count)
            {
                return new PagedCollection<T>(new T[0], totalPages + 1, totalPages, paging.PageSize);
            }

            var result = await query.Skip(skip).Take(paging.PageSize).ToListAsync(cancellationToken);
            return new PagedCollection<T>(result, paging.PageNumber, totalPages, paging.PageSize);
        }
    }
}