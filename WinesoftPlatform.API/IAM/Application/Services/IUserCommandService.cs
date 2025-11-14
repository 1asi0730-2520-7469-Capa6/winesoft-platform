using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Domain.Model;

namespace WinesoftPlatform.API.IAM.Application.Services
{
    // Servicio de comandos de usuario (operaciones que modifican el estado).
    public interface IUserCommandService
    {
        Task CreateAsync(User user);
    }
}
