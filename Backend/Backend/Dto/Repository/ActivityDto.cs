namespace Backend.Dto.Repository
{
    public class ActivityDto
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? RepoName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ActivityCommitDto>? Commits { get; set; } = new List<ActivityCommitDto>();
    }
}
