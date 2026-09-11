using Backend.Data;
using Backend.Interfaces.User;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevHubDbContext _dbContext;
        public UserRepository(DevHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Backend.Models.User?> GetUser(string username)
        {
            return await _dbContext.Users
                .Include(u => u.Repositories)
                .Include(u => u.RepoCommits)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task AddUser(Backend.Models.User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

      
    }
}
