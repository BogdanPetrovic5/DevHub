using Backend.Dto;
using Backend.Responses;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> Register(RegistrationDto registrationDto);
        Task<AuthResponse> Login(LoginDto loginDto);
        Task Logout(string refreshToken);
        Task<AuthResponse> Refresh(string refreshToken);
    }
}
