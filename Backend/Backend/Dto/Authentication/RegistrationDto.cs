using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.Authentication
{
    public class RegistrationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; } = String.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = String.Empty;
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; } = String.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = String.Empty;
        
    }
}
