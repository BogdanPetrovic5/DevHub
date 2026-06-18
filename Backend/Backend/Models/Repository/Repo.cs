





using Backend.Models.Commit;

namespace Backend.Models.Repository
{
    public class Repo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<RepoCommit> RepoCommits { get; set; }
        public ICollection<RepoFile> Files { get; set; }
    }
}

