using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database
{
    public sealed class ContextFactory : IDesignTimeDbContextFactory<ClimbingContext>
    {
        public ClimbingContext CreateDbContext(string[] args)
        {
            return new ClimbingContext(new DbContextOptionsBuilder<ClimbingContext>().UseSqlServer("NOT_A_CONNECTION").Options);
        }
    }
}