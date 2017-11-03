using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Climbing.Web.Database;

namespace Climbing.Web.Database.Postgres
{
    public sealed class ContextFactory : IDesignTimeDbContextFactory<ClimbingContext>
    {
        public ClimbingContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<ClimbingContext>();
            options.UseNpgsql("NOT_A_CONNECTION", b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            return new ClimbingContext(options.Options);
        }
    }
}