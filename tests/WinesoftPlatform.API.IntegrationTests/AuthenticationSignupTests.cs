using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WinesoftPlatform.API.IAM.Infrastructure.Persistence;
using System.Linq;
using System;

namespace WinesoftPlatform.API.IntegrationTests
{
    public class AuthenticationSignupTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthenticationSignupTests(WebApplicationFactory<Program> factory)
        {
            // Ensure JWT secret available for token generation during tests
            Environment.SetEnvironmentVariable("JWT_SECRET", "test_jwt_secret_12345");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Configure IdentityDbContext with in-memory provider for tests
                    services.AddDbContext<IdentityDbContext>(options => { options.UseInMemoryDatabase("TestIdentityDb"); });

                    // Remove existing IUserRepository registration
                    var repoDesc = services.SingleOrDefault(d => d.ServiceType == typeof(WinesoftPlatform.API.IAM.Domain.Repositories.IUserRepository));
                    if (repoDesc != null)
                        services.Remove(repoDesc);

                    services.AddScoped<WinesoftPlatform.API.IAM.Domain.Repositories.IUserRepository, WinesoftPlatform.API.IAM.Infrastructure.Persistence.UserRepository>();
                });
            });
        }

        [Fact]
        public async Task Signup_Valid_Returns201AndSetCookie()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var payload = new { username = "test@example.com", password = "P4ssword123", displayName = "Test" };
            var resp = await client.PostAsJsonAsync("/api/iam/authentication/signup", payload);
            resp.StatusCode.Should().Be(HttpStatusCode.Created);
            resp.Headers.Should().ContainKey("Set-Cookie");
            var setCookie = resp.Headers.GetValues("Set-Cookie").FirstOrDefault();
            setCookie.Should().Contain("ws_auth");
            setCookie.Should().Contain("HttpOnly");
            setCookie.Should().Contain("SameSite=Strict");
        }

        [Fact]
        public async Task Signup_SameUsername_Returns409()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var payload = new { username = "dup@example.com", password = "P4ssword123", displayName = "Test" };
            var resp1 = await client.PostAsJsonAsync("/api/iam/authentication/signup", payload);
            resp1.StatusCode.Should().Be(HttpStatusCode.Created);

            var resp2 = await client.PostAsJsonAsync("/api/iam/authentication/signup", payload);
            resp2.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Theory]
        [InlineData("bademail", "P4ssword123")] // invalid email
        [InlineData("test2@example.com", "short")]
        public async Task Signup_Invalid_Returns400(string username, string password)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var payload = new { username = username, password = password, displayName = "T" };
            var resp = await client.PostAsJsonAsync("/api/iam/authentication/signup", payload);
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
