using Backend.Interfaces.Security;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Backend.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository refreshTokenRepository;
        public TokenService(IConfiguration configuration, ITokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            this.refreshTokenRepository = refreshTokenRepository;
        }
        public string GenerateAccessToken(Models.User user, bool isCli = false)
        {
            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: isCli ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: credentials

            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var range = RandomNumberGenerator.Create();
            range.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

     

        Task<RefreshToken?> ITokenService.GetRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        Task ITokenService.RevokeAllUserTokens(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task ITokenService.RevokeToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        Task ITokenService.SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe)
        {
            throw new NotImplementedException();
        }

        Task ITokenService.SaveRefreshToken(Guid userId, string refreshToken, bool rememberMe, DateTime expDate)
        {
            throw new NotImplementedException();
        }
    }
}
