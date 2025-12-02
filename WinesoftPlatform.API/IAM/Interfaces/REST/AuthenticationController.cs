using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinesoftPlatform.API.IAM.Interfaces.REST.Resources;
using WinesoftPlatform.API.IAM.Infrastructure.Services;
using WinesoftPlatform.API.IAM.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace WinesoftPlatform.API.IAM.Interfaces.REST
{
    // Controller that exposes the authentication endpoint.
    // Forwards signin requests to the external authentication service and returns the received token.
    [ApiController]
    [Route("api/iam/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthHttpClient _authClient;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IWebHostEnvironment _env;

        public AuthenticationController(AuthHttpClient authClient, IAuthService authService, ILogger<AuthenticationController> logger, IWebHostEnvironment env)
        {
            _authClient = authClient;
            _authService = authService;
            _logger = logger;
            _env = env;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var resp = await _authClient.PostAsync<SignInRequest, AuthResponse>(request);
            if (resp == null || string.IsNullOrEmpty(resp.AccessToken))
                return Unauthorized();
            return Ok(resp);
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            // Basic model validation via DataAnnotations
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate username (if it's an email, ensure format)
            var username = request.Username?.Trim();
            if (string.IsNullOrEmpty(username))
                return BadRequest(new { field = "username", message = "Username is required" });

            // Very simple email check: if contains '@' ensure valid email address format
            if (username.Contains("@") && !new EmailAddressAttribute().IsValid(username))
                return BadRequest(new { field = "username", message = "Invalid email format" });

            // Password rules
            var password = request.Password ?? string.Empty;
            if (password.Length < 8 || !System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Za-z]") || !System.Text.RegularExpressions.Regex.IsMatch(password, "[0-9]"))
                return BadRequest(new { field = "password", message = "Password must be at least 8 characters and contain letters and numbers" });

            try
            {
                var (user, token) = await _authService.RegisterAsync(username, password, request.DisplayName);

                // Set cookie with token
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = !_env.IsDevelopment(), // allow non-secure on development
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("ws_auth", token, cookieOptions);

                var resp = new SignUpResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName,
                    AccessToken = null // token deliberately not returned in JSON
                };

                return CreatedAtAction(nameof(SignUp), resp);
            }
            catch (InvalidOperationException ex) when (ex.Message == "UserExists")
            {
                return Conflict(new { message = "User already exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }
    }
}
