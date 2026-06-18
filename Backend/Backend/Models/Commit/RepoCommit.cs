




using Backend.Models.Repository;

namespace Backend.Models.Commit
{
    public class RepoCommit
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid RepositoryId { get; set; }
        public Repo Repository { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<RepoCommitFile> Files { get; set; }
    }
}

