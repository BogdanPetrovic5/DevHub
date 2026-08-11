using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.Authentication
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
