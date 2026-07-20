using Backend.Dto.User;
using Backend.Exceptions;
using Backend.Interfaces.User;
using Backend.Utility;
using System.Threading.Tasks;
namespace Backend.Services.User
{
    public class UserService : IUserService
    {
       private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ProfileDto> GetProfile(string username)
        {
            Backend.Models.User? user = await _userRepository.GetUser(username);
            if (user == null)
            {
                throw new UserNotFoundException($"User with username '{username}' not found.");
            }
            return user.ToDto();
        }
    }
}
