using Backend.Dto;
using Backend.Models;
using Backend.Responses;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<AuthResponse> Register(Models.User user);
        Task<Models.User?> GetUserByEmail(string email);
        Task<bool> EmailExists(string email);
    
        Task<Models.User?> GetUserById(Guid userId);
    }
 }
