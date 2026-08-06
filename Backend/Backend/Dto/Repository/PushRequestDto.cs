namespace Backend.Dto.Repository
{
    public class PushRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public List<PushFileDto> Modified { get; set; } = new();
        public List<PushFileDto> Added { get; set; } = new();
        public List<string> Deleted { get; set; } = new();
    }
}
