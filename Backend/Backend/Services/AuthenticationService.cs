using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Security;
using Backend.Models;
using Backend.Responses;
using Backend.Security;

namespace Backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordEncoder _passwordEncoder;
        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordEncoder passwordEncoder
            ) { 
            _authenticationRepository = authenticationRepository;
            _passwordEncoder = passwordEncoder;
        }
        public async Task<AuthResponse> Register(RegistrationDto registrationDto)
        {
            if (await _authenticationRepository.EmailExists(registrationDto.Email))
                return new AuthResponse { Success = false, Message = "Email already exists" };

            User user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                PasswordHash = _passwordEncoder.EncodePassword(registrationDto.Password)
            };
            return await _authenticationRepository.Register(user);
            
        }
    }
}
