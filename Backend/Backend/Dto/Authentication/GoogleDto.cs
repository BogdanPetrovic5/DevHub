namespace Backend.Dto.Authentication
{
    public class GoogleDto
    {
        public string IdToken { get; set; } = String.Empty;
        public string Nonce { get; set; } = String.Empty;
    }
}
