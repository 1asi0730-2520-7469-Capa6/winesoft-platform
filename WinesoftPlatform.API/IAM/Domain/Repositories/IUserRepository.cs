using System;
using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Domain.Model;

namespace WinesoftPlatform.API.IAM.Domain.Repositories
{
    // Interfaz del repositorio de usuarios (operaciones necesarias por el dominio).
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task CreateAsync(User user);
    }
}
