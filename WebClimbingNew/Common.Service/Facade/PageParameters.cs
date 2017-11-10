namespace Climbing.Web.Common.Service.Facade
{
    public class PageParameters : IPageParameters
    {
        private const int MaxPageSize = 100;

        private const int DefaultPageSize = 10;

        private int pageNumber = 1;

        private int pageSize = DefaultPageSize;

        public int PageNumber
        {
            get => this.pageNumber;
            set => this.pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = value < 1 ? 1 : (value > MaxPageSize ? MaxPageSize : value);
        }
    }
}