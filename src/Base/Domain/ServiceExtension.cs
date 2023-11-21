using Hyperspan.Base.Domain.DbHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspan.Base.Domain
{
    public static class ServiceExtension
    {

        /// <summary>
        /// Connect DB
        /// </summary>
        /// <param name="serviceCollection">Current Instance of IServiceCollection</param>
        /// <param name="connectionString">Connection string from AppSettings.Json</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddDbConnection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            serviceCollection.AddScoped(typeof(IUnitOfWork<,,>), typeof(UnitOfWork<,,>));

            return serviceCollection;
        }
    }
}
