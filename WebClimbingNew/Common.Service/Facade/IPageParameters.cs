namespace Climbing.Web.Common.Service.Facade
{
    public interface IPageParameters
    {
        int PageSize { get; }

        int PageNumber { get; }
    }
}