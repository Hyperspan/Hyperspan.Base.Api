using Domain.DbHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Domain
{
    public static class SqlServerServiceExtensions
    {
        public static void AddSqlServerDbService(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<Context>(builder =>
                GetDbContextOptions(builder, connectionString));
            serviceCollection.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            serviceCollection.AddScoped(typeof(IUnitOfWork<,,>), typeof(UnitOfWork<,,>));
        }

        /// <summary>
        /// Set Database Context Options
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <exception cref="Exception">If the connection string passed is either null or empty.</exception>
        private static void GetDbContextOptions(DbContextOptionsBuilder builder,
            string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ApiErrorException(BaseErrorCodes.NullConnectionString);

            builder.UseSqlServer(connectionString: connectionString,
                options => { options.MigrationsHistoryTable("__EFMigrationsHistory"); });
        }

    }
}
