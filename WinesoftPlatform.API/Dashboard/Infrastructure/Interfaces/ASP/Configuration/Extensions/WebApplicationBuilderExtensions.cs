
using WinesoftPlatform.API.Dashboard.Application.Internal.QueryServices;
using WinesoftPlatform.API.Dashboard.Domain.Services;

namespace WinesoftPlatform.API.Dashboard.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddDashboardContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDashboardQueryService, DashboardQueryService>();
    }
}