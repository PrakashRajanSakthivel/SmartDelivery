using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.common;
using PaymentService.Application.Payment.CommandHandlers;
using Serilog;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Logging;
using Shared.Swagger;
using SharedSvc.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging("paymentservice");

try
{
    Log.Information("Starting up the Payment Service");

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddJwtAuth(builder.Configuration);
    builder.Services.AddSwaggerSupport();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddSingleton<IPaymentService, MockPaymentService>();

    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CreatePaymentIntentCommandHandler).Assembly));

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseDefaultLogging(builder.Configuration);
    app.UseJwtAuth();
    app.UseCors("AllowFrontend");

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

