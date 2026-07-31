using Backend.Models;

namespace Backend.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateAccessToken(Models.User user, bool isCli = false);
        string GenerateRefreshToken();
    }
}
