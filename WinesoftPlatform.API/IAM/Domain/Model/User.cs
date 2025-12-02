using System;

namespace WinesoftPlatform.API.IAM.Domain.Model
{
    // Modelo del dominio que representa datos básicos del usuario.
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Nuevo: hash de la contraseña (no almacenar la contraseña en texto plano)
        public string PasswordHash { get; set; } = string.Empty;

        // Nombre para mostrar
        public string DisplayName { get; set; } = string.Empty;

        // Fecha de creación
        public DateTime CreatedAt { get; set; }

        // Añadir otros campos de perfil según sea necesario (roles, etc.)
    }
}
