using Climbing.Web.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Climbing.Web.Common.Service
{
    public static class InstallServices
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection serviceCollection)
        {
            Guard.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection.AddScoped<IMigrationWaitHelper, MigrationWaitHelper>();

            return serviceCollection;
        }
    }
}