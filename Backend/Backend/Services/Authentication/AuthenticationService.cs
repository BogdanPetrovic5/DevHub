using Backend.Dto.Authentication;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Security;
using Backend.Interfaces.User;
using Backend.Models;
using Backend.Options;
using Backend.Responses;
using Backend.Security;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Backend.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly GoogleOptions _googleOptions;
        private readonly IUserRepository _userRepository;
        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordEncoder passwordEncoder,
            ITokenService tokenService,
            IConfiguration configuration,
            IUserRepository userRepository,
            IOptions<GoogleOptions> googleOptions
            ) { 
            _authenticationRepository = authenticationRepository;
            _passwordEncoder = passwordEncoder;
            _tokenService = tokenService;
            _configuration = configuration;
            _userRepository = userRepository;
            _googleOptions = googleOptions.Value;

        }
        public async Task<AuthResponse> Register(RegistrationDto registrationDto)
        {
            if (await _authenticationRepository.EmailExists(registrationDto.Email))
                return new AuthResponse { Success = false, Message = "Email already exists" };

            Models.User user = new Models.User
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
                response.AccessToken = _tokenService.GenerateAccessToken(user);
                string refreshToken = _tokenService.GenerateRefreshToken();
                response.RefreshToken = refreshToken;

                await _tokenService.SaveRefreshToken(user.Id, refreshToken, false);
            }

            return response;
            
        }

        public async Task<AuthResponse> Login(LoginDto loginDto,bool isCli)
        {
            Models.User? user = await _authenticationRepository.GetUserByEmail(loginDto.Email);

         
            if (user == null || user.IsExternalAuth || !_passwordEncoder.VerifyPassword(loginDto.Password, user.PasswordHash))
                return new AuthResponse { Success = false, Message = "Invalid email or password" };

            
            string accessToken = _tokenService.GenerateAccessToken(user, isCli);
            string refreshToken = _tokenService.GenerateRefreshToken();
            await _tokenService.SaveRefreshToken(user.Id, refreshToken, loginDto.RememberMe);

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
            await _tokenService.RevokeToken(refreshToken);
          
        }

        public async Task<AuthResponse> Refresh(string refreshToken)
        {
            RefreshToken? token = await _tokenService.GetRefreshToken(refreshToken);

            if(token != null && token.IsRevoked)
            {
                await _tokenService.RevokeAllUserTokens(token.UserId);
                return new AuthResponse { Success = false, Message = "Token reuse detected" };
            }
            if(token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return new AuthResponse { Success = false, Message = "Invalid or expired refresh token" };
            }
            Models.User? user = await _authenticationRepository.GetUserById(token.UserId);
            if (user == null) { 
                return new AuthResponse { Success = false, Message = "User not found" };
            }

            string newAccessToken = _tokenService.GenerateAccessToken(user);
            string newRefreshToken = _tokenService.GenerateRefreshToken();
            token.IsRevoked = true;

            await _tokenService.SaveRefreshToken(user.Id, newRefreshToken, token.RememberMe, token.ExpiresAt);
            return new AuthResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RememberMe = token.RememberMe
            };
        }

        public async Task<AuthResponse> GoogleLogin(string idToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleOptions.ClientId }
                });
            }
            catch (Exception)
            {
                return new AuthResponse { Success = false, Message = "Invalid Google token" };
            }
            Models.User? user = await _authenticationRepository.GetUserByEmail(payload.Email);
            if (user == null)
            {
                user = new Models.User
                {
                    Email = payload.Email,
                    Username = payload.Email.Split('@')[0], 
                    IsExternalAuth = true,
                    GoogleId = payload.Subject,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
                };
                await _userRepository.AddUser(user);
            }
            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = _tokenService.GenerateRefreshToken()
            };
        }
    }
}
