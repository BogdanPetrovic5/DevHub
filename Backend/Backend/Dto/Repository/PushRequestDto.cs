namespace Backend.Dto.Repository
{
    public class PushRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public List<PushFileDto> Files { get; set; } = new List<PushFileDto>();
    }
}
