using Serilog;
using CartService.Infra;
using Shared.ServiceDefaults;
using SharedSvc.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("cartservice");

try
{
    Log.Information("Starting up the Cart Service");

    builder.Services
        .AddCartServiceInfrastructure(builder.Configuration);
    builder.Services.AddCustomHealthChecks(new CustomHealthCheckOptions
    {
        ServiceName = "CartService",
        DatabaseConnectionString = builder.Configuration.GetConnectionString("CartDatabase"),
        ElasticsearchUri = builder.Configuration["Elasticsearch:Uri"],
        EnableDatabaseCheck = true,
        EnableElasticsearchCheck = true
    });

    var app = builder.Build();

    app.UseCustomHealthChecks("CartService");
    app.UseServiceDefaults(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}