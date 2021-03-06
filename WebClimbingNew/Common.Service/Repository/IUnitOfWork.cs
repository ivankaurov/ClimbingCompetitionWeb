namespace Climbing.Web.Common.Service.Repository
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IUnitOfWork
    {
        DbSet<T> Repository<T>()
            where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

        ITransaction BeginTransaction();
    }
}