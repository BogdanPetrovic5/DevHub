using Backend.Dto.User;
using Backend.Models;

namespace Backend.Interfaces.User
{
    public interface IUserRepository
    {
        public Task<Backend.Models.User?> GetUser(string username); 
    }
}
