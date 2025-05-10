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

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseDefaultLogging(builder.Configuration);
app.UseJwtAuth();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapDevTokenGenerator(builder.Configuration); // Optional
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
