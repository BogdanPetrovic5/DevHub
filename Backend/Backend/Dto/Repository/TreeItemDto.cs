namespace Backend.Dto.Repository
{
    public class TreeItemDto
    {
        public string Name { get; set; } = String.Empty;
        public string Path { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty; 
        public string LastCommitMessage { get; set; } = String.Empty;
        public DateTime LastCommitAt { get; set; }
    }
}
