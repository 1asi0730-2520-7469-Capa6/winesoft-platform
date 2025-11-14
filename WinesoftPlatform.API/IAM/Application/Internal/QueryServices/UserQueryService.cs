using System;
using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Application.Services;
using WinesoftPlatform.API.IAM.Domain.Model;
using WinesoftPlatform.API.IAM.Domain.Repositories;

namespace WinesoftPlatform.API.IAM.Application.Internal.QueryServices
{
    // Servicio de consultas que delega al repositorio.
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _userRepository.GetByUsernameAsync(username);
        }
    }
}
