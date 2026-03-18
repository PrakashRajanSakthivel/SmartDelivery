using AuthService.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AuthDatabase")));

            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
            services.AddSingleton<IAuthService, AuthService>(sp =>
                new AuthService(sp.GetRequiredService<IConfiguration>()));

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

            return services;
        }
    }
}
