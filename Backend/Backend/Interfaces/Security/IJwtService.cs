using Backend.Models;

namespace Backend.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
