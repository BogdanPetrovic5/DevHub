namespace Backend.Models.Repository
{
    public class RepoFile
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }

        public Guid RepositoryId { get; set; }
        public Repo Repository { get; set; }
    }
}
