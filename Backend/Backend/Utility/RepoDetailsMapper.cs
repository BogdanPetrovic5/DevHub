using Backend.Dto.Repository;
using Backend.Models.Repository;

namespace Backend.Utility
{
    public static class RepoDetailsMapper
    {
        public static RepoDetailsDto ToDetailsDto(
            this Repo repo,
            List<TreeItemDto> tree,
            List<RepoLanguageDto> languages, 
            RepoCommitSummaryDto? latestCommit,
            string currentPath = ""
            ) => new RepoDetailsDto
        {
            Id = repo.Id,
            Name = repo.Name,
            Description = repo.Description,
            IsPrivate = repo.IsPrivate,
            CreatedAt = repo.CreatedAt,
            UpdatedAt = repo.UpdatedAt,
            Languages = languages,
            LatestCommit = latestCommit,
            OwnerUsername = repo.User.Username,
            OwnerId = repo.User.Id,
            CurrentPath = currentPath,
            Tree = tree
        };
    }
}
