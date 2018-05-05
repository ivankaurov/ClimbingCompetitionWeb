namespace Climbing.Web.MigratorService
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Climbing.Web.Common.Service;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Database;
    using Climbing.Web.Database.Postgres;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    internal static class Program
    {
        private static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(30);

        private static AppSettings settings;

        private static IConfiguration configuration;

        private static IServiceProvider serviceProvider;

        private static ILogger logger;

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ExceptionDispatchInfo ex = null;
            try
            {
                MainAsync().GetAwaiter().GetResult();
                Console.WriteLine("Completed succesfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed: {e}");
                ex = ExceptionDispatchInfo.Capture(e);
                logger?.LogCritical(e, "Operation failed: {0}", e.Message);
            }

            Console.WriteLine($"Waiting for {WaitTimeout} to send the results.");
            Thread.Sleep(WaitTimeout);

            if (ex == null)
            {
                Environment.Exit(0);
            }
            else
            {
                ex.Throw();
            }
        }

        private static async Task MainAsync()
        {
            LoadSettings();
            ConfigureServices();
            ConfigureLogging();

            var migrationHelper = serviceProvider.GetRequiredService<IContextHelper>();
            logger.LogInformation("Starting migration");

            await migrationHelper.Migrate(CancellationToken.None);
            logger.LogInformation("Database has the latest version. Starting seeding.");

            var seeder = serviceProvider.GetRequiredService<ISeedingHelper>();
            await seeder.Seed();
            logger.LogInformation("Seeding completed.");
        }

        private static void LoadSettings()
        {
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

            configuration = configBuilder.Build();
            settings = configuration.GetSection("Settings").Get<AppSettings>();
        }

        private static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommonServices()
                .AddCommonDatabaseServices()
                .AddDatabase(settings.ConnectionString)
                .AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            var factory = serviceProvider.GetRequiredService<ILoggerFactory>();
            factory.AddConsole().AddDebug();
            logger = factory.CreateLogger(typeof(Program).FullName);
        }
    }
}
