namespace WinesoftPlatform.API.IAM.Interfaces.REST.Resources
{
    // DTO de petición para signin (username + password).
    public class SignInRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
