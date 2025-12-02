using System;

namespace WinesoftPlatform.API.IAM.Interfaces.REST.Resources
{
    public class SignUpResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? AccessToken { get; set; }
    }
}
