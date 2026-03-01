using AuthService.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUserRepository, HardcodedUserRepository>();
            services.AddSingleton<IAuthService, AuthService>(sp =>
                new AuthService(sp.GetRequiredService<IConfiguration>()));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Singleton;
            });
            return services;
        }
    }
}
