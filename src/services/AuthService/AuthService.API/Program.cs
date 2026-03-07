using AuthService.Infra;
using Shared.CorrelationId;
using Serilog;
using Shared.Logging;
using Shared.Swagger;


var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging("authservice");

try
{
    Log.Information("Starting up the Auth Service");

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
