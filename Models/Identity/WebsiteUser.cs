using QuickTie.Data.Attributes;
using QuickTie.Data.Models;

namespace QuickTie.Portal.Models.Identity
{
    [BsonCollection("websiteUsers")]
    public class WebsiteUser : IDocument
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string NormalizedEmail => Email.ToUpper();

        public bool EmailConfirmed { get; set; } = false;

        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; } = false;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string AccountId { get; set; } = string.Empty;

        public string EmailToken { get;set; } = string.Empty;



        public List<string> Roles { get; set; } = new List<string>();

        public string DisplayName => $"{FirstName} {LastName}";

        
    }
}
