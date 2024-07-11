using Common.Logging;
using Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Product API");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Host.AddAppConfiguration();

    builder.Services.AddInfrastructure();

    var app = builder.Build();

    app.UseInfrastructure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Product API Complete");
    Log.CloseAndFlush();
}


