namespace Climbing.Web.Api.Model
{
    public sealed class Link
    {
        internal Link(string href, string method)
        {
            this.Href = href;
            this.Method = method;
        }

        public string Href { get; }

        public string Method { get; }
    }
}