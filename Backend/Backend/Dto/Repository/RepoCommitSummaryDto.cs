namespace Backend.Dto.Repository
{
    public class RepoCommitSummaryDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AuthorUsername { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public string ShortHash { get; set; } = String.Empty;
    }
}
