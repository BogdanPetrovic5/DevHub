using Backend.Data;
using Backend.Interfaces.Search;
using Backend.Models;
using Backend.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly DevHubDbContext _dbContext;
        public SearchRepository(DevHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<List<Repo>> SearchRepos(string query, Guid userId)
        {
           return _dbContext.Repositories.Include(u=> u.User).Where(r => (!r.IsPrivate || r.User.Id == userId) && r.Name.Contains(query)).Take(10).ToListAsync();
        }

        public Task<List<User>> SearchUsers(string query)
        {
            return _dbContext.Users.Where(u => u.Username.Contains(query)).Take(10).ToListAsync();
        }
    }
}
