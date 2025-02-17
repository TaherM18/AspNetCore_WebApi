using System.ComponentModel.DataAnnotations;

namespace Repositories.Models
{
    public class vm_Login
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? c_Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? c_Password { get; set; }
    }
}