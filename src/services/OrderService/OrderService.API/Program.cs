using Serilog;
using Shared.Http;
using OrderService.Application.Mapper;
using OrderService.Application.Orders.Handlers;
using OrderService.Application.Common;
using OrderService.Infra;
using Shared.CorrelationId;
using Shared.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("orderservice");

try
{
    Log.Information("Starting up the Order Service");

    builder.Services
        .AddOrderServiceInfrastructure(builder.Configuration)
        .AddHttpClients(builder.Configuration);

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));

    builder.Services.AddHttpClient<IRestaurentService, RestaurentService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["RestaurantService:BaseUrl"]!);
    })
    .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
    .AddPolicyHandler(HttpClientPolicies.GetRetryPolicy())
    .AddPolicyHandler(HttpClientPolicies.GetCircuitBreakerPolicy());

    builder.Services.AddAutoMapper(typeof(OrderProfile));

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
