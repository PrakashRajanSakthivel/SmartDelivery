using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Infra;
using Shared.Logging;
using Shared.Swagger;



var builder = WebApplication.CreateBuilder(args);

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
