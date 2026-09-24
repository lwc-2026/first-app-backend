using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests
{
    public class UpdateUserHttpRequest
    {
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
        public string? Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
