using Backend.Dto;
using Backend.Responses;

namespace Backend.Interfaces.Repository
{
    public interface IRepoService
    {
        Task<RepoResponse> Create(CreateRepoDto createRepoDto, Guid userId);
    }
}
