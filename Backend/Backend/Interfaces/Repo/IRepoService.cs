using Backend.Dto;
using Backend.Dto.Repository;
using Backend.Responses;

namespace Backend.Interfaces.Repository
{
    public interface IRepoService
    {
        Task<RepoResponse> Create(CreateRepoDto createRepoDto, Guid userId);
        Task<List<RepoDto>> GetUserRepos(Guid userId);
        Task<RepoDetailsDto?> GetRepo(string username, string repoName, Guid? userId, string path);
        Task<RepoResponse> Upload(Guid repoId,Guid userId, RepoUploadRequest uploadRequest);
        Task<RepoFileContentDto?> GetFileContent(Guid repoId, string path);
        Task<List<RepoCommitSummaryDto>?> GetRepoCommits(Guid? userId, string username, string reponame);
        Task<RepoResponse> Push(Guid repoId, Guid userId, PushRequestDto pushRequest);
    }
}
