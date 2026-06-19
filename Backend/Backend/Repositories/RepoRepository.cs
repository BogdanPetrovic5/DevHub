using Backend.Data;
using Backend.Dto;
using Backend.Interfaces.Repository;
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

        public async Task<List<RepoDto>> GetUserRepos(Guid userId)
        {
            return await _context.Repositories
                .Where(r => r.UserId == userId)
                .Select(r => r.ToDto())
                .ToListAsync();


        }
    }
}
