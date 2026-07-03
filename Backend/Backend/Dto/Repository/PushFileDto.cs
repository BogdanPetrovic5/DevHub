namespace Backend.Dto.Repository
{
    public class PushFileDto
    {
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }
}
