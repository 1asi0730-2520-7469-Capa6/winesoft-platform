using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using WinesoftPlatform.API.IAM.Application.Services;
using WinesoftPlatform.API.IAM.Domain.Model;
using WinesoftPlatform.API.IAM.Domain.Repositories;

namespace WinesoftPlatform.API.IAM.Application.Internal.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserQueryService _userQuery;
        private readonly IUserCommandService _userCommand;
        private readonly IConfiguration _configuration;
        private readonly int _bcryptWorkFactor;

        public AuthService(IUserQueryService userQuery, IUserCommandService userCommand, IConfiguration configuration)
        {
            _userQuery = userQuery;
            _userCommand = userCommand;
            _configuration = configuration;
            _bcryptWorkFactor = int.TryParse(configuration["IAM:BcryptWorkFactor"], out var v) ? v : 10;
        }

        public async Task<(User user, string token)> RegisterAsync(string username, string password, string? displayName)
        {
            // Check uniqueness
            var existing = await _userQuery.GetByUsernameAsync(username);
            if (existing is not null)
                throw new InvalidOperationException("UserExists");

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, _bcryptWorkFactor);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = username,
                PasswordHash = passwordHash,
                DisplayName = displayName ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _userCommand.CreateAsync(user);

            var token = GenerateToken(user);
            return (user, token);
        }

        private string GenerateToken(User user)
        {
            var secret = _configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT secret not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("displayName", user.DisplayName ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

