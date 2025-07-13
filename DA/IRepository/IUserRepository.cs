
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DA.IRepository
{
    public interface IUserRepository<TUser> where TUser : class
    {
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<IdentityResult> CreateAsync(TUser user);
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);
        Task<TUser?> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string code, string password);
        Task<TUser?> FindByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string code);
        Task<IdentityResult> AddLoginAsync(TUser user, ExternalLoginInfo info); // verificar compatibilidad de version de ExternalLoginInfo
        Task<TUser?> GetUserAsync(ClaimsPrincipal user);
        Task ResetAuthenticatorKeyAsync(TUser user);
        Task<string?> GetAuthenticatorKeyAsync(TUser user);
        Task SetTwoFactorEnabledAsync(TUser user, Boolean enabled);
        Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token);
        Task UpdateAsync(TUser user);
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role);
        Task RemoveUser(IdentityUser user);
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);
    }
}
