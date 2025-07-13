using DA.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repository
{
    public class SingInRepository<TUser> : ISingInRepository<TUser> where TUser : class
    {
        private readonly SignInManager<TUser> _sigInManager;

        public SingInRepository(SignInManager<TUser> sigInManager)
        {
            _sigInManager = sigInManager;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, [StringSyntax("Uri")] string? redirectUrl, string? userId = null)
        {
            return _sigInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        public Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return _sigInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null)
        {
            return _sigInManager.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public Task<TUser> GetTwoFactorAuthenticationUserAsync()
        {
            return _sigInManager.GetTwoFactorAuthenticationUserAsync();
        }

        public Task<SignInResult> PasswordSignInAsync(string user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _sigInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null)
        {
            return _sigInManager.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public Task SignOutAsync()
        {
            return _sigInManager.SignOutAsync();
        }

        public Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
           return _sigInManager.TwoFactorAuthenticatorSignInAsync(code,isPersistent,rememberClient);
        }

        public Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            return _sigInManager.UpdateExternalAuthenticationTokensAsync(externalLogin);
        }
    }
}
