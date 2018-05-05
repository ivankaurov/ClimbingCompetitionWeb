namespace Climbing.Web.Api.Model
{
    using System.Collections.Generic;
    using Climbing.Web.Utilities;

    public class PagedResult<T> : LinkedResultBase
    {
        public PagedResult(IEnumerable<T> data)
        {
            Guard.NotNull(data, nameof(data));
            this.Data = data.AsCollection();
        }

        public ICollection<T> Data { get; }
    }
}