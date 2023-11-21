using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Base.Domain.DbHelpers
{
    public interface IRepository<TId, T, TContext>
        where TId : IEquatable<TId>
        where T : class
        where TContext : DbContext
    {
        IQueryable<T> Entities { get; }

        Task<int> GetCount();

        Task<int> GetCount(string sqlQuery);

        Task<T?> GetById(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(string sqlQuery);

        Task<T> AddAsync(T entity);

        Task<bool> AddRangeAsync(List<T> entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);
    }
}

