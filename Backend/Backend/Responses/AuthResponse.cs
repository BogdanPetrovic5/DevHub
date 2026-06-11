namespace Backend.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
    };
}
