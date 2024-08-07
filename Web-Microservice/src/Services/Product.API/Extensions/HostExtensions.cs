using Microsoft.EntityFrameworkCore;
using Mysqlx.Prepare;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Product.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.Information("Migrating mysql database");

                    ExecuteMigrations(context);

                    logger.Information("Migrated mysql database");

                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception ex)
                {
                    logger.Information(ex, "An error occured when migrate mysql database");
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
        {
            seeder(context, services);
        }

        private static void ExecuteMigrations<TContext>(TContext? context) where TContext : DbContext
        {
            context.Database.Migrate();
        }
    }
}
