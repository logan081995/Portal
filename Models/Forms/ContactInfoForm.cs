using QuickTie.Portal.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuickTie.Portal.Models
{
    public class ContactInfoForm
    {
        [Required(ErrorMessage = "Enter a first name.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a last name.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a email.")]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a address.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a city.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a state.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a valid zip code.")]
        [RegularExpression(RegularExpressions.ZipCode, ErrorMessage = "Invalid zip code")]
        public string PostalCode { get; set; } = string.Empty;

        public string? Company { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Apartment { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public bool EmailOffers { get; set; } = false;

        public string CombinedAddress => $"{Address}, {City}, {PostalCode}, {Country}";
    }
}