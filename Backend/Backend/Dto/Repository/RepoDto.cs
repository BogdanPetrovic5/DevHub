namespace Backend.Dto.Repository
{
    public class RepoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerUsername { get; set; } = string.Empty;
    }
}
