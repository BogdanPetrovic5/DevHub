namespace Backend.Dto
{
    public class CreateRepoDto
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        public bool IsPrivate { get; set; } = false;

    }
}
