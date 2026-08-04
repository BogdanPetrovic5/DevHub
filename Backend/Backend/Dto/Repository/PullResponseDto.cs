namespace Backend.Dto.Repository
{
    public class PullResponseDto
    {
        public List<PullFileResponseDto> Modified { get; set; } = new();
        public List<PullFileResponseDto> Added { get; set; } = new();
        public List<string> Deleted { get; set; } = new();
    }
}
