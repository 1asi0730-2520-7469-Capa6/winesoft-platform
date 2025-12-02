using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Domain.Model;

namespace WinesoftPlatform.API.IAM.Application.Services
{
    public interface IAuthService
    {
        Task<(User user, string token)> RegisterAsync(string username, string password, string? displayName);
    }
}
