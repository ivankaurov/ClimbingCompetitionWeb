namespace Climbing.Web.Api.Model
{
    public sealed class Link
    {
        public Link(LinkType rel, string href, string method)
        {
            this.Href = href;
            this.Method = method;
            this.Rel = rel;
        }
        public LinkType Rel { get; }
        public string Href { get; }
        public string Method { get; }
    }
}