using Climbing.Web.Api.Model;
using Climbing.Web.Common.Service.Facade;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Climbing.Web.Api.Utilites
{
    internal static class PagedResultFactory
    {
        public static PagedResult<TResult> ToPagedResult<TResult>(this IPagedCollection<TResult> pagedCollection, IUrlHelper urlHelper, string getRouteName)
        {
            Guard.NotNull(pagedCollection, nameof(pagedCollection));
            Guard.NotNull(urlHelper, nameof(urlHelper));
            Guard.NotNullOrWhitespace(getRouteName, nameof(getRouteName));

            var result = new PagedResult<TResult>(pagedCollection.Page);
            if(pagedCollection.PageNumber > 1)
            {
                result.AddLink(
                    LinkType.PreviousPage,
                    urlHelper.Link(getRouteName, new PageParameters { PageNumber = pagedCollection.PageNumber - 1, PageSize = pagedCollection.PageSize }),
                    "GET");
            }

            if(pagedCollection.PageNumber < pagedCollection.TotalPages)
            {
                result.AddLink(
                    LinkType.NextPage,
                    urlHelper.Link(getRouteName, new PageParameters { PageNumber = pagedCollection.PageNumber + 1, PageSize = pagedCollection.PageSize }),
                    "GET");
            }

            return result;
        }
    }
}