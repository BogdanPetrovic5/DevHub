using Backend.Dto.User;

namespace Backend.Utility
{
    public static class ProfileMapper
    {
        public static ProfileDto ToDto(this Backend.Models.User user) => new ProfileDto
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RepoCount = user.Repositories.Count,
            CommitCount = user.RepoCommits.Count,
            Repositories = user.Repositories.Select(r => r.ToDto()).ToList()

        };
    }
}
