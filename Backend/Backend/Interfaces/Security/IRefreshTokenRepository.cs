using Backend.Models;

namespace Backend.Interfaces.Security
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe);
        Task<RefreshToken?> GetRefreshToken(string refreshToken);
        Task RevokeAllUserTokens(Guid userId);
        Task RevokeToken(string refreshToken);

    }
}
