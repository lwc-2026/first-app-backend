using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests
{
    public class CreateUserHttpRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
        public string Username { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; } = string.Empty;
    }
}
