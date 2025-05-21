using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Infra;
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
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "orderservice-logs-{0:yyyy.MM.dd}",
        CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                           EmitEventFailureHandling.RaiseCallback |
                           EmitEventFailureHandling.ThrowException
    })
    .CreateLogger();

try
{
    Log.Information("Starting up the Order Service");
    builder.Host.UseSerilog();

    builder.Services
        .AddControllers();

    builder.Services
        .AddEndpointsApiExplorer()
        .AddOrderServiceInfrastructure(builder.Configuration)
        .AddHttpClients(builder.Configuration)
        .AddJwtAuth(builder.Configuration)
        .AddSwaggerSupport();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    var app = builder.Build();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        };
        options.GetLevel = (ctx, elapsed, ex) =>
            ex != null ? LogEventLevel.Error :
            ctx.Response.StatusCode > 499 ? LogEventLevel.Error :
            LogEventLevel.Information;
    });

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseDefaultLogging(builder.Configuration);
    app.UseJwtAuth();

    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapDevTokenGenerator(builder.Configuration); // Optional
    //app.MapGet("/", [ApiExplorerSettings(IgnoreApi = true)] () => Results.Redirect("/swagger/index.html"));
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }
        await next();
    });
    //}

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
