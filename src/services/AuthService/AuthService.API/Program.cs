using AuthService.Infra;
using Serilog;
using Shared.ServiceDefaults;
using SharedSvc.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var defaults = new ServiceDefaultsOptions { UseJwt = false, UseCors = true, MapDevToken = false };
builder.AddServiceDefaults("authservice", defaults);

try
{
    Log.Information("Starting up the Auth Service");

    builder.Services.AddAuthServiceInfrastructure(builder.Configuration);
    builder.Services.AddCustomHealthChecks(new CustomHealthCheckOptions
    {
        ServiceName = "AuthService",
        DatabaseConnectionString = builder.Configuration.GetConnectionString("AuthDatabase"),
        ElasticsearchUri = builder.Configuration["Elasticsearch:Uri"],
        EnableDatabaseCheck = true,
        EnableElasticsearchCheck = true
    });

    var app = builder.Build();

    app.UseCustomHealthChecks("AuthService");
    app.UseServiceDefaults(builder.Configuration, defaults);

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
