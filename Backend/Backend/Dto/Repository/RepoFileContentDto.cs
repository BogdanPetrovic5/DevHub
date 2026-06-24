namespace Backend.Dto.Repository
{
    public class RepoFileContentDto
    {
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Language {  get; set; } = string.Empty;
    }
}