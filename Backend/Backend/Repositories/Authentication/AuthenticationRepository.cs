using Backend.Data;
using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Responses;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DevHubDbContext _devHubDbContext;
        public AuthenticationRepository(DevHubDbContext devHubDbContext) { 
            _devHubDbContext = devHubDbContext;
        }

        public async Task<AuthResponse> Register(User user)
        {
            try
            {
                await _devHubDbContext.Users.AddAsync(user);
                bool result =  _devHubDbContext.SaveChanges() > 0;
                return new AuthResponse
                {
                    Success = result,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred while registering the user"
                };
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _devHubDbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _devHubDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _devHubDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
