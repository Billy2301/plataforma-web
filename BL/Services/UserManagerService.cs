using BL.IServices;
using DA.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserManagerService<TUser> : IUserManagerService<TUser> where TUser : class
    {
        private readonly IUserRepository<TUser> _userRepository;
        private readonly IGenericRepositoryCentral<TUser> _gericRepository;
        public UserManagerService(IUserRepository<TUser> userRepository, IGenericRepositoryCentral<TUser> gericRepository)
        {
            _userRepository = userRepository;
            _gericRepository = gericRepository;
        }

        public Task<IdentityResult> AddLoginAsync(TUser user, ExternalLoginInfo info)
        {
            return _userRepository.AddLoginAsync(user, info);
        }

        public Task<IdentityResult> AddToRoleAsync(TUser user, string role)
        {
            return _userRepository.AddToRoleAsync(user, role);
        }

        public Task<IdentityResult> ConfirmEmailAsync(TUser user, string code)
        {
            return _userRepository.ConfirmEmailAsync(user, code);
        }

        public Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            return _userRepository.CreateAsync(user, password);
        }

        public Task<IdentityResult> CreateAsync(TUser user)
        {
            return _userRepository.CreateAsync(user);
        }

        public Task<TUser?> FindByEmailAsync(string email)
        {
            return _userRepository.FindByEmailAsync(email);
        }

        public Task<TUser> FindByIdAsync(string id)
        {
            return _userRepository.FindByIdAsync(id);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        {
            return _userRepository.GenerateEmailConfirmationTokenAsync(user);
        }

        public Task<string> GeneratePasswordResetTokenAsync(TUser user)
        {
            return _userRepository.GeneratePasswordResetTokenAsync(user);
        }

        public Task<string?> GetAuthenticatorKeyAsync(TUser user)
        {
            return _userRepository.GetAuthenticatorKeyAsync(user);
        }

        public Task<TUser?> GetUserAsync(ClaimsPrincipal user)
        {
            return _userRepository.GetUserAsync(user);
        }

        public Task ResetAuthenticatorKeyAsync(TUser user)
        {
            return _userRepository.ResetAuthenticatorKeyAsync(user);
        }

        public Task<IdentityResult> ResetPasswordAsync(TUser user, string code, string password)
        {
            return _userRepository.ResetPasswordAsync(user, code, password);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            return _userRepository.SetTwoFactorEnabledAsync(user, enabled);
        }

        public Task UpdateAsync(TUser user)
        {
            return _userRepository.UpdateAsync(user);
        }

        public Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token)
        {
            return _userRepository.VerifyTwoFactorTokenAsync(user, tokenProvider, token);
        }

        public async Task<List<TUser>> ListarUsuarios()
        {
            IQueryable<TUser> query = await _gericRepository.Consultar();
            return query.ToList();
        }

        public Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role)
        {
            return _userRepository.RemoveFromRoleAsync(user, role);
        }

        public Task RemoveUser(IdentityUser user)
        {
            return _userRepository.RemoveUser(user);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return await _userRepository.GetLoginsAsync(user);
        }
    }
}
