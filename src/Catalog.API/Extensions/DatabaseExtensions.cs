using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Catalog.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddCatalogContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<CatalogContext>(opt =>
                {
                    opt.UseSqlServer(
                        connectionString,
                        serverOptions => {
                            serverOptions.MigrationsAssembly(typeof(Startup)
                                .GetTypeInfo()
                                .Assembly
                                .GetName()
                                .Name);
                        });
                });
        }
    }
}
