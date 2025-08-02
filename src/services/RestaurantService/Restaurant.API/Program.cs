using RestaurantService.Application.Mapper;
using RestaurantService.Infra.Data;
using RestaurentService.Infra.Data;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Logging;
using Shared.Swagger;
using SharedSvc.Infra.Restaurant;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddRestaurentServiceInfrastructure(builder.Configuration)
    .AddHttpClients(builder.Configuration)
    .AddJwtAuth(builder.Configuration)
    .AddSwaggerSupport();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAutoMapper(typeof(RestaurantProfile));

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseDefaultLogging(builder.Configuration);
app.UseJwtAuth();

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
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
