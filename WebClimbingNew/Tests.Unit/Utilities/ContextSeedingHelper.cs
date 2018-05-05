namespace Climbing.Web.Tests.Unit.Utilities
{
    using System.Linq;
    using Climbing.Web.Common.Service.Repository;
    using Climbing.Web.Model;

    internal sealed class ContextSeedingHelper
    {
        private static readonly object SyncRoot = new object();

        private static bool seedingCompleted;

        private readonly IUnitOfWork context;

        public ContextSeedingHelper(IUnitOfWork context)
        {
            this.context = context;
        }

        public void Seed()
        {
            if (seedingCompleted)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (seedingCompleted)
                {
                    return;
                }

                var pt = this.context.Repository<Team>().Add(new Team { Name = Team.RootTeamName, Code = Team.RootTeamCode });

                this.context.Repository<Team>().AddRange(
                    Enumerable.Range(1, 11)
                              .Select(i => new Team { Name = $"Team_{i:00}", Code = $"{i:00}", Parent = pt.Entity }));

                this.context.SaveChangesAsync().GetAwaiter().GetResult();
                seedingCompleted = true;
            }
        }
    }
}