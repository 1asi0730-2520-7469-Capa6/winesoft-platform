using System;
using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Domain.Model;

namespace WinesoftPlatform.API.IAM.Application.Services
{
    // Servicio de consultas de usuario (solo lectura desde la capa de aplicación).
    public interface IUserQueryService
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
    }
}
