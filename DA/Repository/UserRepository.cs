using DA.CentralContext;
using DA.IRepository;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DA.Repository
{
    public class UserRepository<TUser> : IUserRepository<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly UserManager<IdentityUser> _userManagers;
        public readonly CPALCentralContext _dbContext;
        public UserRepository(UserManager<TUser> userManager, UserManager<IdentityUser> userManagers, CPALCentralContext dbContext)
        {
            _userManager = userManager;
            _userManagers = userManagers;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> AddLoginAsync(TUser user, ExternalLoginInfo info)
        {
            return await _userManager.AddLoginAsync(user, info);
        }

        public async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
        {
           return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(TUser user, string code)
        {
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(TUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<TUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<TUser?> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(TUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string?> GetAuthenticatorKeyAsync(TUser user)
        {
            return await _userManager.GetAuthenticatorKeyAsync(user);
        }

        public async Task<TUser?> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task ResetAuthenticatorKeyAsync(TUser user)
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(TUser user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }

        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }

        public async Task UpdateAsync(TUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token)
        {
            return await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user,role);
        }

        public async Task RemoveUser(IdentityUser user)
        {
             _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return await _userManager.GetLoginsAsync(user);
        }
    }
}
