using System;

namespace WinesoftPlatform.API.IAM.Domain.Model
{
    // Modelo del dominio que representa datos básicos del usuario.
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Añadir otros campos de perfil según sea necesario (nombre, roles, etc.)
    }
}
