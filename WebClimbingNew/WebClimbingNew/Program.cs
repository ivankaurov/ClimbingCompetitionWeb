﻿namespace Climbing.Web.Portal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

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
            using (var scope = serviceProvider.CreateScope())
            {
                var helper = scope.ServiceProvider.GetRequiredService<IMigrationWaitHelper>();
                var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                await helper.WaitForMigrationsToComplete(settings.MigrationWaitTimeout, settings.MigrationPollInterval, CancellationToken.None);
            }
        }
    }
}
