using Backend.Models.Commit;
using Backend.Models.Repository;

namespace Backend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public List<Repo> Repositories { get; set; } = new List<Repo>();
        public List<RepoCommit> RepoCommits { get; set; } = new List<RepoCommit>();

    }
}
