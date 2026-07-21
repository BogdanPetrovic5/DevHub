using Backend.Data;
using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Responses;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DevHubDbContext _devHubDbContext;
        public AuthenticationRepository(DevHubDbContext devHubDbContext) { 
            _devHubDbContext = devHubDbContext;
        }

        public async Task<AuthResponse> Register(Models.User user)
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
            catch (DbUpdateException)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email or username already exists"
                };
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _devHubDbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<Models.User?> GetUserByEmail(string email)
        {
            return await _devHubDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    

        public async Task<Models.User?> GetUserById(Guid userId)
        {
            return await _devHubDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
