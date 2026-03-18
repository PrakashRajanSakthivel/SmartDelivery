using PaymentService.Infra;
using Serilog;
using Shared.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("paymentservice");

try
{
    Log.Information("Starting up the Payment Service");

    builder.Services.AddPaymentServiceInfrastructure(builder.Configuration);

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

