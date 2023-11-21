using Hyperspan.Shared;
using Hyperspan.Shared.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Hyperspan.Base.Domain.DbHelpers
{
    public class UnitOfWork<TId, T, TContext> : IUnitOfWork<TId, T, TContext>, IDisposable
        where TId : IEquatable<TId>
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private bool _disposed;
        private readonly Hashtable _repositories;

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ApiErrorException(BaseErrorCodes.ArgumentNull);
            _repositories ??= new Hashtable();
        }

        public IRepository<TId, TEntity, TContext>? Repository<TEntity>() where TEntity : BaseEntity<TId>
        {
            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                var output = _repositories[type];
                if (output == null) return null;

                return (IRepository<TId, TEntity, TContext>)_repositories[type];
            }

            var repositoryType = typeof(Repository<,,>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

            _repositories.Add(type, repositoryInstance);

            return (IRepository<TId, TEntity, TContext>)_repositories[type];
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> StartTransaction(bool checkIfAlreadyExists = false)
        {
            if (!checkIfAlreadyExists) return await _dbContext.Database.BeginTransactionAsync();

            if (_dbContext.Database.CurrentTransaction != null)
                return _dbContext.Database.CurrentTransaction;

            return await _dbContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            _dbContext.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task Rollback(bool handleOnlyIfExists = false)
        {
            if (handleOnlyIfExists && _dbContext.Database.CurrentTransaction == null)
                return Task.CompletedTask;

            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            _dbContext.Database.RollbackTransaction();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }

            //dispose unmanaged resources
            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose();
        }

    }
}
