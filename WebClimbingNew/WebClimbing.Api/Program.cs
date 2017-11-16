using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Climbing.Web.Database;
using Climbing.Web.Common.Service;
using System.Threading;

namespace Climbing.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            Console.WriteLine("Migrating...");
            WaitForMigrations(host.Services).GetAwaiter().GetResult();
            Console.WriteLine("Migration completed");
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
                var settings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                if(settings.MigrateByApi)
                {
                    var migrator = scope.ServiceProvider.GetRequiredService<IContextHelper>();
                    await migrator.Migrate(CancellationToken.None);
                }
                else
                {
                    var helper = scope.ServiceProvider.GetRequiredService<IMigrationWaitHelper>();
                    await helper.WaitForMigrationsToComplete(settings.MigrationWaitTimeout,settings.MigrationPollInterval, CancellationToken.None);
                }
            }
        }
    }
}
