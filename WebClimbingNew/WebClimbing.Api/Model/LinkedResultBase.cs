using System.Collections.Generic;

namespace Climbing.Web.Api.Model
{
    public abstract class LinkedResultBase
    {
        public ICollection<Link> Links { get; } = new List<Link>();

        public void AddLink(LinkType rel, string href, string httpMethod) => this.Links.Add(new Link(rel, href, httpMethod));
    }
}