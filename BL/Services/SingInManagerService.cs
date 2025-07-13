using BL.IServices;
using DA.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class SingInManagerService<TUser> : ISingInManagerService<TUser> where TUser : class
    {
        private readonly ISingInRepository<TUser> _singInRepository;

        public SingInManagerService(ISingInRepository<TUser> singInRepository)
        {
            _singInRepository = singInRepository;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, [StringSyntax("Uri")] string? redirectUrl, string? userId = null)
        {
            return _singInRepository.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        public Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return _singInRepository.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null)
        {
            return _singInRepository.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public Task<TUser> GetTwoFactorAuthenticationUserAsync()
        {
            return _singInRepository.GetTwoFactorAuthenticationUserAsync();
        }

        public Task<SignInResult> PasswordSignInAsync(string user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _singInRepository.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null)
        {
            return _singInRepository.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public Task SignOutAsync()
        {
            return _singInRepository.SignOutAsync();
        }

        public Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
            return _singInRepository.TwoFactorAuthenticatorSignInAsync(code, isPersistent, rememberClient);
        }

        public Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            return _singInRepository.UpdateExternalAuthenticationTokensAsync(externalLogin);
        }
    }
}
