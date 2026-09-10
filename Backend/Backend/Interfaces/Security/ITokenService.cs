using Backend.Models;

namespace Backend.Interfaces.Security
{
    public interface ITokenService
    {
        string GenerateAccessToken(Models.User user, bool isCli = false);
        string GenerateRefreshToken();

        Task SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe);
        Task SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe, DateTime expDate);
        Task<RefreshToken?> GetRefreshToken(string refreshToken);
        Task RevokeAllUserTokens(Guid userId);
        Task RevokeToken(string refreshToken);

    }
}
