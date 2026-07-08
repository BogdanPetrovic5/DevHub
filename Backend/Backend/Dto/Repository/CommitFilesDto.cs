namespace Backend.Dto.Repository
{
    public class CommitFilesDto
    {
        public string AuhtorUsername { get; set; } = string.Empty;
        public string CommitMessage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<RepoCommitFileDto> Files { get; set; } = new List<RepoCommitFileDto>();
    }
}
