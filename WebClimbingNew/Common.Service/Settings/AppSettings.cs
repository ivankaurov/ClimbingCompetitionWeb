namespace Climbing.Web.Common.Service
{
    using System;

    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public TimeSpan MigrationWaitTimeout { get; set; }

        public TimeSpan MigrationPollInterval { get; set; }

        public bool MigrateByApi { get; set; }
    }
}