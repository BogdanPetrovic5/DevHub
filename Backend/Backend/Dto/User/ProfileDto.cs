using Backend.Dto.Repository;

namespace Backend.Dto.User
{
    public class ProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RepoCount { get; set; }
        public int CommitCount { get; set; }
        public List<RepoDto> Repositories { get; set; } = new();
    }

}
