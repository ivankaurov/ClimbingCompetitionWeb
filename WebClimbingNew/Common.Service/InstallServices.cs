namespace Climbing.Web.Common.Service
{
    using Climbing.Web.Common.Service.Facade;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Utilities;
    using Microsoft.Extensions.DependencyInjection;

    public static class InstallServices
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection serviceCollection)
        {
            Guard.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection.AddScoped<IMigrationWaitHelper, MigrationWaitHelper>()
                .AddScoped<ITeamsService, TeamsService>()
                .AddScoped<ISeedingHelper, SeedingHelper>();

            return serviceCollection;
        }
    }
}