namespace Climbing.Web.Database.Postgres
{
    using System;
    using System.Reflection;
    using Climbing.Web.Database;
    using Climbing.Web.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class InstallServices
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
        {
            Guard.NotNull(serviceCollection, nameof(serviceCollection));
            Guard.NotNullOrWhitespace(connectionString, nameof(connectionString));
            Console.WriteLine($"ConnectionString={connectionString}");
            serviceCollection.AddDbContextPool<ClimbingContext>(opt => opt.UseNpgsql(connectionString, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
            return serviceCollection;
        }
    }
}