using QuickTie.Portal.Models;
using QuickTie.Portal.Models.Identity;

namespace QuickTie.Portal.Data.Identity.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<WebsiteUser>> GetUsersAsync();
        Task UpdateUserAsync(WebsiteUser updatedUser);
    }
}
