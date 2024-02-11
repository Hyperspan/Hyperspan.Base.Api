using Domain.DbHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Domain
{
    public static class Postgres_ServiceExtension
    {
        public static IServiceCollection AddPostgresDbService(this IServiceCollection serviceCollection,
            string connectionString)
        {
            serviceCollection.AddDbContext<Context>(builder =>
                               GetDbContextOptions(builder, connectionString));
            serviceCollection.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            serviceCollection.AddScoped(typeof(IUnitOfWork<,,>), typeof(UnitOfWork<,,>));
            return serviceCollection;
        }


        private static void GetDbContextOptions(DbContextOptionsBuilder builder,
            string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ApiErrorException(BaseErrorCodes.NullConnectionString);

            builder.UseNpgsql(connectionString);
        }
    }
}
