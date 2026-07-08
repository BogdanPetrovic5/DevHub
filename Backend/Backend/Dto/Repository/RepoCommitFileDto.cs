namespace Backend.Dto.Repository
{
    public class RepoCommitFileDto
    {
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ChangeType { get; set; } = string.Empty;
    }
}
