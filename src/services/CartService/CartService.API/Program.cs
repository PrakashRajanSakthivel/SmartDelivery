using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Logging;
using Shared.Swagger;
using CartService.Infra;


var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging("cartservice");

try
{
    Log.Information("Starting up the Cart Service");

    builder.Services
        .AddControllers();

    builder.Services
        .AddEndpointsApiExplorer()
        .AddCartServiceInfrastructure(builder.Configuration)
        .AddHttpClients(builder.Configuration)
        .AddJwtAuth(builder.Configuration)
        .AddSwaggerSupport();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseDefaultLogging(builder.Configuration);
    app.UseJwtAuth();
    app.UseCors("AllowFrontend");

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