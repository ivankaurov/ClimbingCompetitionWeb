using Microsoft.EntityFrameworkCore;

using Climbing.Web.Model;
using Climbing.Web.Model.Logging;
using Climbing.Web.Database.EntityBuilderExtensions;

namespace Climbing.Web.Database
{
    public class ClimbingContext : DbContext
    {
        public ClimbingContext(DbContextOptions<ClimbingContext> options) : base(options)
        {
        }

        public DbSet<Ltr> LogicTransactions { get; set; }

        public DbSet<LtrObject> LtrObjects { get; set; }
        public DbSet<LtrObjectProperties> LtrObjectProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapLtr()
                        .MapLtrObject()
                        .MapLtrObjectProperties();
            base.OnModelCreating(modelBuilder);
        }
    }

}
