using Backend.Dto.Repository;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Backend.Responses;
using System.Reflection.Metadata.Ecma335;

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

        Task<List<RepoCommit>> GetRecentCommits(Guid userId);

        Task SaveUpload(List<RepoFile> files, RepoCommit commit, List<RepoCommitFile> commitFiles);
        Task<RepoFile?> GetFileContent(Guid repoId, string path);
        Task<List<RepoCommit>?> GetRepoCommits(Guid repoId);
        
        Task SavePush(List<RepoFile> toInsert, List<RepoFile> toUpdate, List<RepoFile> toDelete, RepoCommit repoCommit, List<RepoCommitFile> commitFiles);
        Task <List<RepoCommitFile>?> GetCommitFilesByCommitId(Guid commitId);

        
    }
}
