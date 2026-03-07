using AuthService.Infra;
using Serilog;
using Shared.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

var defaults = new ServiceDefaultsOptions { UseJwt = false, UseCors = true, MapDevToken = false };
builder.AddServiceDefaults("authservice", defaults);

try
{
    Log.Information("Starting up the Auth Service");

    builder.Services.AddAuthServiceInfrastructure(builder.Configuration);

    var app = builder.Build();

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
