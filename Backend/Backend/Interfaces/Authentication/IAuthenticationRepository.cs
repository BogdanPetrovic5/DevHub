using Backend.Dto;
using Backend.Models;
using Backend.Responses;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<AuthResponse> Register(User user);
        Task<User?> GetUserByEmail(string email);
        Task<bool> EmailExists(string email);
        Task<AuthResponse> Logout(Guid userId);
    }
}
