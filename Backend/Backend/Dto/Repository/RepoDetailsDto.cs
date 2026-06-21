namespace Backend.Dto.Repository
{
    public class RepoDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OwnerUsername { get; set; } = String.Empty;
        public Guid OwnerId { get; set; }
        public List<RepoLanguageDto> Languages { get; set; }
        public RepoCommitSummaryDto? LatestCommit { get; set; }
        public List<TreeItemDto> Tree { get; set; }
        public string CurrentPath { get; set; } = String.Empty;
    }
}
