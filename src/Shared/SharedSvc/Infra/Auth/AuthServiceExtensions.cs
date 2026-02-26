using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Infra;

namespace SharedSvc.Infra.Auth
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthServiceInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return AuthService.Infra.ServiceCollectionExtensions.AddAuthServiceInfrastructure(services, configuration);
        }
    }
}
