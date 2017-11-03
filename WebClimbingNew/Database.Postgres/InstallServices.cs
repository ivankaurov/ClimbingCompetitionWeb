using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Climbing.Web.Database;
using Climbing.Web.Utilities;

namespace Climbing.Web.Database.Postgres
{
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