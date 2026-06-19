using Backend.Dto;
using Backend.Models.Repository;
using Backend.Responses;

namespace Backend.Interfaces.Repository
{
    public interface IRepoRepository
    {
        Task<RepoResponse> Create(Repo repository);
        Task<List<RepoDto>> GetUserRepos(Guid userId);
    }
}
