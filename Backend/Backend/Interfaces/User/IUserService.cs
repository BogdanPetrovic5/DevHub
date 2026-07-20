using Backend.Dto.User;

namespace Backend.Interfaces.User
{
    public interface IUserService
    {
        public Task<ProfileDto> GetProfile(string username);

    }
}
