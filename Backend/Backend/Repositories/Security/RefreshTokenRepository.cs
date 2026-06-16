using Backend.Data;
using Backend.Interfaces.Security;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Security
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DevHubDbContext _context;

        public RefreshTokenRepository(DevHubDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task RevokeAllUserTokens(Guid userId)
        {
            var tokens = _context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked);
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }
           await _context.SaveChangesAsync();
        }

        public async Task SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpiresAt = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                RememberMe = rememberMe

            };
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
    }
}
