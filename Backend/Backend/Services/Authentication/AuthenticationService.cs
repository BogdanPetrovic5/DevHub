using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Security;
using Backend.Models;
using Backend.Responses;
using Backend.Security;

namespace Backend.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordEncoder passwordEncoder,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository
            ) { 
            _authenticationRepository = authenticationRepository;
            _passwordEncoder = passwordEncoder;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<AuthResponse> Register(RegistrationDto registrationDto)
        {
            if (await _authenticationRepository.EmailExists(registrationDto.Email))
                return new AuthResponse { Success = false, Message = "Email already exists" };

            User user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                PasswordHash = _passwordEncoder.EncodePassword(registrationDto.Password)
            };
            AuthResponse response = await _authenticationRepository.Register(user);

            if (response.Success)
            {
                response.AccessToken = _jwtService.GenerateAccessToken(user);
                string refreshToken = _jwtService.GenerateRefreshToken();
                response.RefreshToken = refreshToken;

                await _refreshTokenRepository.SaveRefreshToken(user.Id, refreshToken, false);
            }

            return response;
            
        }

        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            User? user = await _authenticationRepository.GetUserByEmail(loginDto.Email);

            if (user == null || !_passwordEncoder.VerifyPassword(loginDto.Password, user.PasswordHash))
                return new AuthResponse { Success = false, Message = "Invalid email or password" };

            string accessToken = _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();
            await _refreshTokenRepository.SaveRefreshToken(user.Id, refreshToken, loginDto.RememberMe);

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task Logout(string refreshToken)
        {
            await _refreshTokenRepository.RevokeToken(refreshToken);
          
        }

        public async Task<AuthResponse> Refresh(string refreshToken)
        {
            RefreshToken? token = await _refreshTokenRepository.GetRefreshToken(refreshToken);

            if(token != null && token.IsRevoked)
            {
                await _refreshTokenRepository.RevokeAllUserTokens(token.UserId);
                return new AuthResponse { Success = false, Message = "Token reuse detected" };
            }
            if(token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return new AuthResponse { Success = false, Message = "Invalid or expired refresh token" };
            }
            User? user = await _authenticationRepository.GetUserById(token.UserId);
            if (user == null) { 
                return new AuthResponse { Success = false, Message = "User not found" };
            }

            string newAccessToken = _jwtService.GenerateAccessToken(user);
            string newRefreshToken = _jwtService.GenerateRefreshToken();
            token.IsRevoked = true;

            await _refreshTokenRepository.SaveRefreshToken(user.Id, newRefreshToken, token.RememberMe);
            return new AuthResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RememberMe = token.RememberMe
            };
        }
    }
}
