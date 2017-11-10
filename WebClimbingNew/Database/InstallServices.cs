using Climbing.Web.Common.Service;
using Climbing.Web.Common.Service.Repository;
using Climbing.Web.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Climbing.Web.Database
{
    public static class InstallServices
    {
        public static IServiceCollection AddCommonDatabaseServices(this IServiceCollection serviceCollection)
        {
            Guard.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection.AddScoped<IContextHelper, ClimbingContextHelper>()
                             .AddScoped<IUnitOfWork>(s => s.GetRequiredService<ClimbingContext>());
            return serviceCollection;
        }
    }
}