namespace Backend.Interfaces.Security
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe);
    }
}
