using Backend.Dto.Repository;
using Backend.Models.Commit;

namespace Backend.Utility
{
    public static class CommitMapper
    {
        public static RepoCommitSummaryDto ToSummaryDto(this RepoCommit commit) => new RepoCommitSummaryDto
        {
            Id = commit.Id,
            Message = commit.Message,
            AuthorUsername = commit.User.Username,
            CreatedAt = commit.CreatedAt,
            ShortHash = commit.Id.ToString("N")[..7]// i am using the first 7 chars and removing the dashes
        };
    }
}
