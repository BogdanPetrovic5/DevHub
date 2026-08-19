using Backend.Dto.Search;
using Backend.Models;
using Backend.Models.Repository;

namespace Backend.Utility
{
    public static class SearchMapper
    {
        public static RepoSearchItemDto ToSearchDto(this Repo repo) => new RepoSearchItemDto
        {
            OwnerUsername = repo.User.Username,
            RepoName = repo.Name
        };

        public static UserSearchItemDto ToSearchDto(this User user) => new UserSearchItemDto
        {
            Username = user.Username
        };
    }
}
