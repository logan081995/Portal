using QuickTie.Portal.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuickTie.Portal.Models
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "Enter a email")]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter a first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Your account ID is required to register.")]
        [Display(Name = "Account ID")]
        public string AccountId { get; set; } = string.Empty;
    }
}
