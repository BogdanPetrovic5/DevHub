namespace Backend.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = String.Empty;
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public bool RememberMe { get; set; } = false;
    };
}
