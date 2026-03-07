using Serilog;
using Shared.ServiceDefaults;
using PaymentService.Application.common;
using PaymentService.Application.Payment.CommandHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("paymentservice");

try
{
    Log.Information("Starting up the Payment Service");

    builder.Services.AddSingleton<IPaymentService, MockPaymentService>();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CreatePaymentIntentCommandHandler).Assembly));

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

