using System;

namespace Climbing.Web.Common.Service
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public TimeSpan MigrationWaitTimeout { get; set; }

        public TimeSpan MigrationPollInterval { get; set; }
    }
}