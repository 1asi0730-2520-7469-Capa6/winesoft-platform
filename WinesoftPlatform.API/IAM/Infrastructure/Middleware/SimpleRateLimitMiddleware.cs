using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace WinesoftPlatform.API.IAM.Infrastructure.Middleware
{
    public class SimpleRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SimpleRateLimitMiddleware> _logger;
        private readonly int _limit;
        private readonly TimeSpan _window;

        public SimpleRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<SimpleRateLimitMiddleware> logger, int limit = 5, TimeSpan? window = null)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _limit = limit;
            _window = window ?? TimeSpan.FromMinutes(1);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (!path.StartsWith("/api/iam/authentication", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = $"rl_{path}_{ip}";

            var entry = _cache.GetOrCreate(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = _window;
                return new RateLimitEntry { Count = 0 };
            });

            if (entry.Count >= _limit)
            {
                _logger.LogWarning("Rate limit exceeded for {Ip} on {Path}", ip, path);
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers["Retry-After"] = ((int)_window.TotalSeconds).ToString();
                await context.Response.WriteAsync("Too Many Requests");
                return;
            }

            entry.Count++;
            _cache.Set(key, entry, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = _window });

            await _next(context);
        }

        private class RateLimitEntry { public int Count { get; set; } }
    }

    public static class SimpleRateLimitMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleRateLimit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SimpleRateLimitMiddleware>();
        }
    }
}

