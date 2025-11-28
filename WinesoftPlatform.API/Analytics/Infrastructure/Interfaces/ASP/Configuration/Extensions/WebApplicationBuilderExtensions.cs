
using WinesoftPlatform.API.Analytics.Application.Internal.QueryServices;
using WinesoftPlatform.API.Analytics.Domain.Services;

namespace WinesoftPlatform.API.Analytics.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddAnalyticsContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
    }
}