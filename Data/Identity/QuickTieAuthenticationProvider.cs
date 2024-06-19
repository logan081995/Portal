using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using QuickTie.Data.Models;
using QuickTie.Portal.Models;
using QuickTie.Portal.Models.Identity;
using QuickTie.Services.Interface;
using System.Security.Claims;

namespace QuickTie.Portal.Data.Identity
{
    public class WebsiteAuthenticator : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly IMongoRepository<WebsiteUser> _repository;
        private readonly IMongoRepository<Account> _acctRepository;

        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public readonly NavigationManager _navManager;

        public WebsiteAuthenticator(ProtectedLocalStorage protectedLocalStorage, IMongoRepository<WebsiteUser> repository, IMongoRepository<Account> acctRepository, NavigationManager navManager)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _repository = repository;
            _acctRepository = acctRepository;
            _navManager = navManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                //await Task.Delay(5000);
                var userSessionStorageResult = await _protectedLocalStorage.GetAsync<WebsiteUser>("identity");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                var claimsPrincipal = new ClaimsPrincipal(CreateIdentityFromUser(userSession));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task LoginAsync(LoginFormModel loginFormModel)
        {
            var (userInDatabase, isSuccess) = await LookUpUserAsync(loginFormModel.Email, loginFormModel.Password);
            var principal = _anonymous;

            if (isSuccess)
            {
                var identity = CreateIdentityFromUser(userInDatabase);

                principal = new ClaimsPrincipal(identity);
                await _protectedLocalStorage.SetAsync("identity", userInDatabase);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            await _protectedLocalStorage.DeleteAsync("identity");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        public async Task<bool> RegisterUserAsync(RegisterForm registerModel)
        {
            // Find out if Account ID exists
            var account = await _acctRepository.FindByIdAsync(registerModel.AccountId);

            if (account == null)
            {
                return false;
            }

            var newUser = new WebsiteUser
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = registerModel.AccountId,
                UserName = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                EmailConfirmed = false
            };

            newUser.PasswordHash = new PasswordHasher<WebsiteUser>().HashPassword(newUser, registerModel.Password);

            // Create user and forward to login
            await _repository.InsertOneAsync(newUser);

            return true;
        }

        private static ClaimsIdentity CreateIdentityFromUser(WebsiteUser user)
        {
            return new ClaimsIdentity(new Claim[]
            {
            new (ClaimTypes.Name, user.DisplayName),
            new (ClaimTypes.Hash, user.PasswordHash),
            new (ClaimTypes.Role, user.Roles.Any() ? user.Roles.First() : "ProductEditor"),
            new("AccountId", user.AccountId)
            }, "QuickTieAuth");
        }

        private async Task<(WebsiteUser, bool)> LookUpUserAsync(string username, string password)
        {
            
            var userQuery = _repository.GetQueryable();
            userQuery = (MongoDB.Driver.Linq.IMongoQueryable<WebsiteUser>)userQuery.Where(x => x.Email.ToLower().Equals(username.ToLower()));
            var result = await _repository.FindByFilterAsync(userQuery);

            if (result.Any()) {
                // Validate password
                PasswordVerificationResult validPassword = new PasswordHasher<WebsiteUser>().VerifyHashedPassword(result.First(), result.First().PasswordHash, password);
                return (result.First(), validPassword == PasswordVerificationResult.Success);
            }

            return (null, false);
        }
    }
}
