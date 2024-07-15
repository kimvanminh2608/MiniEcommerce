using Microsoft.EntityFrameworkCore;

namespace Customer.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating postgreSQL database");

                    ExecuteMigrations(context);

                    logger.LogInformation("Migrated postgreSQL database");

                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured when migrate postgreSQL database");
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
