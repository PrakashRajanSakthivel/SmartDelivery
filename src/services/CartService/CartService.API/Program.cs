using Serilog;
using Shared.Http;
using CartService.Infra;
using Shared.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("cartservice");

try
{
    Log.Information("Starting up the Cart Service");

    builder.Services
        .AddCartServiceInfrastructure(builder.Configuration)
        .AddHttpClients(builder.Configuration);

    var app = builder.Build();

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