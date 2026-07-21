using Backend.Models;

namespace Backend.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateAccessToken(Models.User user);
        string GenerateRefreshToken();
    }
}
