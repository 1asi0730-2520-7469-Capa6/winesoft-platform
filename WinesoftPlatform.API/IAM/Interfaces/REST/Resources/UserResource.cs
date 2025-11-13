using System;

namespace WinesoftPlatform.API.IAM.Interfaces.REST.Resources
{
    // Representación simplificada del usuario usada en respuestas REST.
    public class UserResource
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
