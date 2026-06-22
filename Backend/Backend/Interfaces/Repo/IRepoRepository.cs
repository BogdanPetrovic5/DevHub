using Backend.Dto.Repository;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Backend.Responses;

namespace Backend.Interfaces.Repository
{
    public interface IRepoRepository
    {
        Task<RepoResponse> Create(Repo repository);
        Task<List<RepoDto>> GetUserRepos(Guid userId);
        Task<Repo?> GetByUsernameAndName(string username, string repoName);
        Task<Repo?> GetByIdAndOwner(Guid repoId, Guid userId);
        Task<List<RepoFile>> GetFiles(Guid repoId);
        Task<List<RepoCommitFile>> GetCommitFiles(Guid repoId);
        Task<RepoCommit?> GetLatestCommit(Guid repoId);
        Task SaveUpload(List<RepoFile> files, RepoCommit commit, List<RepoCommitFile> commitFiles);
    }
}
