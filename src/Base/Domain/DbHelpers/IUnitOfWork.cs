using Hyperspan.Shared.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hyperspan.Base.Domain.DbHelpers
{
    public interface IUnitOfWork<TId, T, TContext>
        where TId : IEquatable<TId>
        where T : class
        where TContext : DbContext
    {

        IRepository<TId, T, TContext> Repository<T>() where T : BaseEntity<TId>;

        Task<int> Save(CancellationToken cancellationToken);

        Task<IDbContextTransaction> StartTransaction(bool checkIfAlreadyExists = false);

        Task Commit();

        Task Rollback(bool handleOnlyIfExists = false);

    }
}
