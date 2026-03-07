using RestaurantService.Application.Mapper;
using RestaurantService.Infra.Data;
using RestaurentService.Infra.Data;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Logging;
using Shared.Swagger;
using RestaurantService.Infra;
using SharedSvc.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var elasticUri = configuration["Elasticsearch:Uri"];

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri ?? "http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "restaurantservice-logs-{0:yyyy.MM.dd}",
        CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                           EmitEventFailureHandling.RaiseCallback
    })
    .CreateLogger();

try
{
    Log.Information("Starting up the Restaurant Service");
    builder.Host.UseSerilog();

    builder.Services
        .AddControllers();

    builder.Services
        .AddEndpointsApiExplorer()
        .AddRestaurentServiceInfrastructure(builder.Configuration)
        .AddHttpClients(builder.Configuration)
        .AddJwtAuth(builder.Configuration)
        .AddSwaggerSupport()
        .AddCustomHealthChecks(new CustomHealthCheckOptions
        {
            ServiceName = "RestaurantService",
            DatabaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),
            ElasticsearchUri = builder.Configuration["Elasticsearch:Uri"],
            EnableDatabaseCheck = true,
            EnableElasticsearchCheck = true
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.AddAutoMapper(typeof(RestaurantProfile));

    var app = builder.Build();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseDefaultLogging(builder.Configuration);
    app.UseJwtAuth();
    app.UseCors("AllowFrontend");

    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<RestaurantDbContext>();
            SeedData.Initialize(dbContext);
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapDevTokenGenerator(builder.Configuration);
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger/index.html");
                return;
            }
            await next();
        });
    }

    app.UseCustomHealthChecks("RestaurantService");
    app.UseHttpsRedirection();
    app.MapControllers();
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
