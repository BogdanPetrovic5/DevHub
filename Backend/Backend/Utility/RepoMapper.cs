using Backend.Dto;
using Backend.Models.Repository;

namespace Backend.Utility
{
    public static class RepoMapper
    {

        public static RepoDto ToDto(this Repo repo) => new RepoDto
        {
            Id = repo.Id,
            Name = repo.Name,
            Description = repo.Description,
            IsPrivate = repo.IsPrivate,
            CreatedAt = repo.CreatedAt
        };
    }
}
