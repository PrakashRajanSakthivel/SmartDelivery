using RestaurantService.Application.Mapper;
using RestaurantService.Infra.Data;
using RestaurentService.Infra.Data;
using RestaurantService.Infra;
using SharedSvc.HealthChecks;
using Serilog;
using Shared.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults("restaurantservice");

try
{
    Log.Information("Starting up the Restaurant Service");

    builder.Services
        .AddRestaurentServiceInfrastructure(builder.Configuration)
        .AddCustomHealthChecks(new CustomHealthCheckOptions
        {
            ServiceName = "RestaurantService",
            DatabaseConnectionString = builder.Configuration.GetConnectionString("RestaurantDatabase"),
            ElasticsearchUri = builder.Configuration["Elasticsearch:Uri"],
            EnableDatabaseCheck = true,
            EnableElasticsearchCheck = true
        });

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.AddAutoMapper(typeof(RestaurantProfile));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        SeedData.Initialize(dbContext);
    }

    app.UseCustomHealthChecks("RestaurantService");
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
