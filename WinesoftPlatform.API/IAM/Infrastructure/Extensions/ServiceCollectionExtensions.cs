using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinesoftPlatform.API.IAM.Application.Internal.CommandServices;
using WinesoftPlatform.API.IAM.Application.Internal.QueryServices;
using WinesoftPlatform.API.IAM.Application.Services;
using WinesoftPlatform.API.IAM.Domain.Repositories;
using WinesoftPlatform.API.IAM.Infrastructure.Persistence;
using WinesoftPlatform.API.IAM.Infrastructure.Services;

namespace WinesoftPlatform.API.IAM.Infrastructure.Extensions
{
    // Extensión para registrar los servicios del módulo IAM en DI.
    // Claves de configuración útiles:
    // - IAM:AuthBaseUrl -> URL base del servicio de autenticación externo.
    // - IAM:SigninPath  -> Ruta del endpoint signin (opcional, por defecto /api/auth/signin).
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIAM(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["IAM:AuthBaseUrl"];
            if (!string.IsNullOrWhiteSpace(baseUrl) && Uri.TryCreate(baseUrl, UriKind.Absolute, out var _))
            {
                services.AddHttpClient("AuthClient", client => client.BaseAddress = new Uri(baseUrl));
            }
            else
            {
                services.AddHttpClient("AuthClient");
            }

            services.AddSingleton<AuthHttpClient>();

            services.AddScoped<IUserCommandService, UserCommandService>();
            services.AddScoped<IUserQueryService, UserQueryService>();

            services.AddSingleton<IUserRepository, InMemoryUserRepository>();

            return services;
        }
    }
}
