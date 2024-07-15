using Common.Logging;
using Customer.API.Controllers;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Customer API");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    // Add services to the container.
    builder.Host.AddAppConfiguration();

    builder.Services.AddInfrastructure(builder.Configuration);
    

    var app = builder.Build();

    app.UseInfrastructure();

    app.MapCustomersAPI();

    app.SeedCustomerData().Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Customer API Complete");
    Log.CloseAndFlush();
}


