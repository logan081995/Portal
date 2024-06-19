using QuickTie.Portal.Data.Identity.Interface;
using QuickTie.Portal.Models;
using QuickTie.Portal.Models.Identity;
using QuickTie.Services.Interface;
using QuickTie.Services.Services;

namespace QuickTie.Portal.Data.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<WebsiteUser> _userRepository;
        private readonly EmailService _emailService;

        public UserService(IMongoRepository<WebsiteUser> userRepository, EmailService emailSerivce)
        {
            _userRepository = userRepository;
            _emailService = emailSerivce;
        }

        public async Task<IEnumerable<WebsiteUser>> GetUsersAsync()
        {
            var orderQuery = _userRepository.GetQueryable();
            return await _userRepository.FindByFilterAsync(orderQuery);
        }

        public async Task UpdateUserAsync(WebsiteUser updatedUser)
        {
            await _userRepository.ReplaceOneAsync (updatedUser);
        }
    }
}