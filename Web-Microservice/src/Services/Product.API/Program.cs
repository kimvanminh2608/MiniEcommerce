using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Product API");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Host.AddAppConfiguration();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseInfrastructure();

    // auto migrate database when run api
    app.MigrateDatabase<ProductContext>((context, _) =>
        {
            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
        })
        .Run();
    
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException",StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Product API Complete");
    Log.CloseAndFlush();
}


