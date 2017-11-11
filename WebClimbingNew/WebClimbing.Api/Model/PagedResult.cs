using System.Collections;
using System.Collections.Generic;
using Climbing.Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Climbing.Web.Api.Model
{
    public class PagedResult<T> : LinkedResultBase, IEnumerable<T>
    {
        public PagedResult(IEnumerable<T> data)
        {
            Guard.NotNull(data, nameof(data));
            this.Data = data.AsCollection();
        }

        public ICollection<T> Data { get; }

        public IEnumerator<T> GetEnumerator() => this.Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}