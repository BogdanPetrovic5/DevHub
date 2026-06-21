using Backend.Data;
using Backend.Dto.Repository;
using Backend.Interfaces.Repository;
using Backend.Models;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Backend.Responses;
using Backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepoRepository : IRepoRepository
    {
        private readonly DevHubDbContext _context;
        public RepoRepository(DevHubDbContext context)
        {
            _context = context;
        }
        public async Task<RepoResponse> Create(Repo repository)
        {
            try
            {
                _context.Repositories.Add(repository);
                await _context.SaveChangesAsync();

                return new RepoResponse
                {
                    Message = "Successfully created a new repository!",
                    Success = true
                };
            }
            catch (DbUpdateException)
            {
                return new RepoResponse
                {
                    Success = false,
                    Message = "You already have a repository with that name"
                };
            }

        }

        public Task<Repo?> GetByUsernameAndName(string username, string repoName)
        {
            return _context.Repositories.Include(r => r.User)
                .Where(r => r.User.Username == username && r.Name == repoName)
                .FirstOrDefaultAsync();
               
        }

        public Task<List<RepoFile>> GetFiles(Guid repoId)
        {
            return _context.RepoFiles
                .Where(f => f.RepositoryId == repoId)
                .ToListAsync(); 
        }

        public Task<RepoCommit?> GetLatestCommit(Guid repoId)
        {
            return _context.RepoCommits
                .Include(c => c.User)
                .Where(c => c.RepositoryId == repoId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

       

        public async Task<List<RepoDto>> GetUserRepos(Guid userId)
        {
            var repos = await _context.Repositories
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return repos.Select(r => r.ToDto()).ToList();


        }
    }
}
