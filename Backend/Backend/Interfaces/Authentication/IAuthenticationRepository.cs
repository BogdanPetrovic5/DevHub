using Backend.Dto;
using Backend.Models;
using Backend.Responses;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<AuthResponse> Register(User user);
        Task<bool> EmailExists(string email);
    }
}
