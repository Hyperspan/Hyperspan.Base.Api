using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared;
using Shared.Config;

namespace Domain.DbHelpers
{
    /// <summary>
    /// Generic repository for performing CRUD operations on entities using Entity Framework Core.
    /// </summary>
    /// <typeparam name="TId">Type of the entity ID.</typeparam>
    /// <typeparam name="T">Type of the entity.</typeparam>
    /// <typeparam name="TContext">Type of the database context.</typeparam>
    public class Repository<TId, T, TContext> : IRepository<TId, T, TContext>
        where TId : IEquatable<TId>
        where T : class, IBaseEntity<TId>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public TContext Context => _dbContext;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the queryable entities for the specified type.
        /// </summary>
        public IQueryable<T> Entities => _dbContext.Set<T>().Where(x => !x.IsDeleted);

        /// <summary>
        /// Gets the count of entities in the repository.
        /// </summary>
        public Task<int> GetCount()
        {
            return Task.FromResult(Entities
                .Count(x => !x.IsDeleted));
        }


        /// <summary>
        /// Gets the count of entities in the repository based on a custom SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query to retrieve the count.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<int> GetCount(string sqlQuery)
        {
            using var objCommand = _dbContext.Database.GetDbConnection().CreateCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = System.Data.CommandType.Text;

            if (objCommand.Connection != null && objCommand.Connection.State != System.Data.ConnectionState.Open)
                objCommand.Connection.Open();
            if (_dbContext.Database.CurrentTransaction != null)
            {
                objCommand.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();
            }

            var intCount = Convert.ToInt32(objCommand.ExecuteScalar());

            objCommand.Dispose();

            return Task.FromResult(intCount);
        }


        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<T?> GetById(TId id)
        {
            var record = await _dbContext.Set<T>().FindAsync(id);
            return record?.IsDeleted == true ? null : record;
        }


        /// <summary>
        /// Retrieves all entities from the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiErrorException">Thrown if the query fails.</exception>
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbContext
                    .Set<T>()
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.QueryFailed, e);
            }
        }

        /// <summary>
        /// Retrieves entities from the repository based on a custom SQL query asynchronously.
        /// </summary>
        /// <param name="sqlQuery">The SQL query to retrieve entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiErrorException">Thrown if the query fails.</exception>
        public async Task<List<T>> GetAllAsync(string sqlQuery)
        {
            try
            {
                return await _dbContext
                    .Set<T>()
                    .FromSqlRaw(sqlQuery)
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.QueryFailed, e);
            }
        }

        /// <summary>
        /// Adds an entity to the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiErrorException">Thrown if the insertion fails.</exception>
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.CreatedOn = DateTime.UtcNow;
                await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.InsertFailed, e);
            }
        }

        /// <summary>
        /// Adds a range of entities to the repository asynchronously.
        /// </summary>
        /// <param name="entities">The list of entities to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiErrorException">Thrown if the insertion fails.</exception>
        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            try
            {
                entities.ForEach(x => { x.CreatedOn = DateTime.UtcNow; });

                await _dbContext.Set<T>().AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.InsertFailed, e);
            }
        }

        /// <summary>
        /// Updates an entity in the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiErrorException">Thrown if the update fails.</exception>
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                if (entity.Id == null || string.IsNullOrEmpty(entity.Id.ToString())
                                      || entity.Id.ToString() == Guid.Empty.ToString())
                    throw new ApiErrorException(BaseErrorCodes.InvalidId);

                var exist = await _dbContext.Set<T>().FindAsync(entity.Id);
                if (exist == null || exist.IsDeleted) throw new ApiErrorException(BaseErrorCodes.RecordNotFound);

                entity.LastModifiedOn = DateTime.UtcNow;
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.UpdateFailed, e);
            }
        }

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                // Validate the entity ID
                if (entity.Id == null || string.IsNullOrEmpty(entity.Id.ToString())
                                      || entity.Id.ToString() == Guid.Empty.ToString())
                    throw new ApiErrorException(BaseErrorCodes.InvalidId);

                // Find the existing entity
                var exist = await _dbContext.Set<T>().FindAsync(entity.Id);

                if (exist == null || exist.IsDeleted) throw new ApiErrorException(BaseErrorCodes.RecordNotFound);

                // Update values and save changes
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.DeleteFailed, e);
            }
        }
    }
}