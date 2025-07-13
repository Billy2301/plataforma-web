using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.IRepository
{
    public interface ISingInRepository<TUser> where TUser : class
    {
        Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null);
        Task<SignInResult> PasswordSignInAsync(string user, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, [StringSyntax(StringSyntaxAttribute.Uri)] string? redirectUrl, string? userId = null);
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin);
        Task<TUser> GetTwoFactorAuthenticationUserAsync();
        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient);
    }
}
