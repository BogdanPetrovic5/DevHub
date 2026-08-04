namespace Backend.Dto.Repository
{
    public class PullRequestDto
    {
        public List<PullFileDto> Files { get; set; } = new List<PullFileDto>();
    }
}
