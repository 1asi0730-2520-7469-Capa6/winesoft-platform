namespace WinesoftPlatform.API.IAM.Interfaces.REST.Resources
{
    // DTO de respuesta de autenticación esperado por el controlador.
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public UserResource? User { get; set; }
    }
}
