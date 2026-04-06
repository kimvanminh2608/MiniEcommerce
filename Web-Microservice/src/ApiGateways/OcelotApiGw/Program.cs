using Common.Logging;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);
Log.Information("Start API Gateway");
try
{
    // Add services to the container.
    builder.Host.AddAppConfiguration();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.ConfigureCors(builder.Configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("CorsPolicy");

    app.UseMiddleware<ErrorWrappingMiddleware>();

    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseEndpoints(e =>
    //{
    //    e.MapGet("/", async context =>
    //    {
    //        await context.Response.WriteAsync($"Tedu class");
    //    });
    //});
    app.UseEndpoints(e =>
    {
        e.MapGet("/", context =>
        {
            context.Response.Redirect("swagger/index.html");
            return Task.CompletedTask;
        });
    });
    app.MapControllers();

    app.UseSwaggerForOcelotUI(o =>
    {
        o.PathToSwaggerGenerator = "swagger/docs";
    });
    await app.UseOcelot();

    app.Run();
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
    Log.Information("Shut down API Gateway");
    Log.CloseAndFlush();
}

