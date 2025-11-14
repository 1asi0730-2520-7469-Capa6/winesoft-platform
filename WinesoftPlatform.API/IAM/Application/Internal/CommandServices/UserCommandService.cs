using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Application.Services;
using WinesoftPlatform.API.IAM.Domain.Model;
using WinesoftPlatform.API.IAM.Domain.Repositories;

namespace WinesoftPlatform.API.IAM.Application.Internal.CommandServices
{
    // Servicio que implementa operaciones que modifican el estado del usuario.
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;

        public UserCommandService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(User user)
        {
            // Aquí se pueden aplicar reglas de dominio antes de persistir.
            await _userRepository.CreateAsync(user);
        }
    }
}
