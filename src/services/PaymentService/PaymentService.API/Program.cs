using PaymentService.Infra;
using Serilog;
using Shared.ServiceDefaults;
using SharedSvc.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("paymentservice");

try
{
    Log.Information("Starting up the Payment Service");

    builder.Services.AddPaymentServiceInfrastructure(builder.Configuration);
    builder.Services.AddCustomHealthChecks(new CustomHealthCheckOptions
    {
        ServiceName = "PaymentService",
        DatabaseConnectionString = builder.Configuration.GetConnectionString("PaymentDatabase"),
        ElasticsearchUri = builder.Configuration["Elasticsearch:Uri"],
        EnableDatabaseCheck = true,
        EnableElasticsearchCheck = true
    });

    var app = builder.Build();

    app.UseCustomHealthChecks("PaymentService");
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

