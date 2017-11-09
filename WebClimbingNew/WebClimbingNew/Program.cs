using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Climbing.Web.Common.Service;
using Climbing.Web.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Climbing.Web.Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            Console.WriteLine("Migrating...");
            WaitForMigrations(host.Services).GetAwaiter().GetResult();
            Console.WriteLine("Migrations completed");

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static async Task WaitForMigrations(IServiceProvider serviceProvider)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var helper = scope.ServiceProvider.GetRequiredService<IMigrationWaitHelper>();
                var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                await helper.WaitForMigrationsToComplete(settings.MigrationWaitTimeout,settings.MigrationPollInterval, CancellationToken.None);
            }
        }
    }
}
