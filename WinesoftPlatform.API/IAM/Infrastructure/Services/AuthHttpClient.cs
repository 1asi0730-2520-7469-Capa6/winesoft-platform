using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WinesoftPlatform.API.IAM.Infrastructure.Services
{
    // Lightweight HTTP client to call an external authentication service.
    // Uses IHttpClientFactory named "AuthClient" and the following configuration keys:
    // - IAM:AuthBaseUrl -> base URL of the external authentication service.
    // - IAM:SigninPath  -> signin endpoint path (defaults to "/api/auth/signin").
    public class AuthHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _signinPath;

        public AuthHttpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _signinPath = configuration["IAM:SigninPath"] ?? "/api/auth/signin";
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(TRequest payload)
        {
            var client = _httpClientFactory.CreateClient("AuthClient");
            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var resp = await client.PostAsync(_signinPath, content);
            if (!resp.IsSuccessStatusCode)
                return default;
            var respJson = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(respJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
