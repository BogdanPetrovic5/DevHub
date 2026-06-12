using Backend.Data;
using Backend.Interfaces.Security;
using Backend.Models;

namespace Backend.Repositories.Security
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DevHubDbContext _context;

        public RefreshTokenRepository(DevHubDbContext context)
        {
            _context = context;
        }

        public async Task SaveRefreshToken(Guid userId, string refreshToken)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
    }
}
