namespace Backend.Models.Commit
{
    public class RepoCommitFile
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string ChangeType { get; set; }  

        public Guid CommitId { get; set; }
        public RepoCommit Commit { get; set; }
    }
}
