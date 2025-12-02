using System.ComponentModel.DataAnnotations;

namespace WinesoftPlatform.API.IAM.Interfaces.REST.Resources
{
    public class SignUpRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? DisplayName { get; set; }
    }
}
