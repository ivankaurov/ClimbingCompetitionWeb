namespace Climbing.Web.Api.Model
{
    using System.Collections.Generic;

    public abstract class LinkedResultBase
    {
        public IDictionary<LinkType, Link> Links { get; } = new Dictionary<LinkType, Link>();

        public Link AddLink(LinkType rel, string href, string httpMethod) => this.Links[rel] = new Link(href, httpMethod);
    }
}