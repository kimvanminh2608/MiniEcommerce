using Basket.API.Extensions;
using Common.Logging;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(Serilogger.Configure);
    Log.Information("Start Basket API");

    builder.Services.ConfigureServices();
    builder.Services.ConfigureRedis(builder.Configuration);
    var app = builder.Build();

    app.MapControllers();
    app.UseInfrastructure();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Basket API Complete");
    Log.CloseAndFlush();
}


