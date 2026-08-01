using Backend.Dto.Authentication;
using Backend.Responses;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> Register(RegistrationDto registrationDto);
        Task<AuthResponse> Login(LoginDto loginDto, bool isCli = false);
        Task Logout(string refreshToken);
        Task<AuthResponse> Refresh(string refreshToken);

    }
}
