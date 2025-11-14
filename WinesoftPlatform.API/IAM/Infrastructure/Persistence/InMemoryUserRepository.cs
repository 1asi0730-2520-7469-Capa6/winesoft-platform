using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WinesoftPlatform.API.IAM.Domain.Model;
using WinesoftPlatform.API.IAM.Domain.Repositories;

namespace WinesoftPlatform.API.IAM.Infrastructure.Persistence
{
    // In-memory repository used for development and testing.
    // Do not use in production; replace with an EF Core or persistent implementation.
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _store = new ConcurrentDictionary<Guid, User>();

        public Task CreateAsync(User user)
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            _store[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            foreach (var user in _store.Values)
            {
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
                    return Task.FromResult<User?>(user);
            }
            return Task.FromResult<User?>(null);
        }
    }
}
