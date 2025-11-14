using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinesoftPlatform.API.IAM.Interfaces.REST.Resources;
using WinesoftPlatform.API.IAM.Infrastructure.Services;

namespace WinesoftPlatform.API.IAM.Interfaces.REST
{
    // Controller that exposes the authentication endpoint.
    // Forwards signin requests to the external authentication service and returns the received token.
    [ApiController]
    [Route("api/iam/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthHttpClient _authClient;

        public AuthenticationController(AuthHttpClient authClient)
        {
            _authClient = authClient;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var resp = await _authClient.PostAsync<SignInRequest, AuthResponse>(request);
            if (resp == null || string.IsNullOrEmpty(resp.AccessToken))
                return Unauthorized();
            return Ok(resp);
        }
    }
}
