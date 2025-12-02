using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WinesoftPlatform.API.IAM.Domain.Model;
using WinesoftPlatform.API.IAM.Domain.Repositories;

namespace WinesoftPlatform.API.IAM.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;

        public UserRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(User user)
        {
            if (user.Id == Guid.Empty) user.Id = Guid.NewGuid();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }
    }
}

