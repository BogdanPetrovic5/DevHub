namespace Backend.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public bool RememberMe { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
