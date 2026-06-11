using Backend.Interfaces.Security;
using Microsoft.AspNetCore.Identity;

namespace Backend.Security
{
    public class PasswordEncoder : IPasswordEncoder
    {
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        public string EncodePassword(string password)
        {

            return _passwordHasher.HashPassword(String.Empty, password);
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(String.Empty, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
