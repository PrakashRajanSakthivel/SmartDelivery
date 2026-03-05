using AuthService.Infra;
using Shared.CorrelationId;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Shared.Logging;
using Shared.Swagger;


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
        IndexFormat = "authservice-logs-{0:yyyy.MM.dd}",
        CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                           EmitEventFailureHandling.RaiseCallback
    })
    .CreateLogger();

try
{
    Log.Information("Starting up the Auth Service");
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCorrelationIdSupport();

    // Register AuthService infrastructure
    builder.Services.AddAuthServiceInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseDefaultLogging(builder.Configuration);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

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
