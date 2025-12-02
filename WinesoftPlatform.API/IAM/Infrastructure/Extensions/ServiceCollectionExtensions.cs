using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WinesoftPlatform.API.IAM.Application.Internal.CommandServices;
using WinesoftPlatform.API.IAM.Application.Internal.QueryServices;
using WinesoftPlatform.API.IAM.Application.Services;
using WinesoftPlatform.API.IAM.Application.Internal.Services;
using WinesoftPlatform.API.IAM.Domain.Repositories;
using WinesoftPlatform.API.IAM.Infrastructure.Persistence;
using WinesoftPlatform.API.IAM.Infrastructure.Services;

namespace WinesoftPlatform.API.IAM.Infrastructure.Extensions
{
    // Extension to register IAM module services into DI.
    // Useful configuration keys:
    // - IAM:AuthBaseUrl -> base URL of the external authentication service.
    // - IAM:SigninPath  -> signin endpoint path (optional, defaults to /api/auth/signin).
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

            // By default use in-memory repository. If configuration enables EF, register IdentityDbContext and EF repository.
            if (string.Equals(configuration["IAM:UseEf"], "true", StringComparison.OrdinalIgnoreCase))
            {
                // IdentityDbContext should be registered by the host (Program.cs) with the desired provider.
                services.AddScoped<IUserRepository, UserRepository>();
            }
            else
            {
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            }

            // Registrar servicio de autenticación local (registro, tokens)
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
