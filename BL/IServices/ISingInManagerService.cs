using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface ISingInManagerService<TUser> where TUser : class
    {
        /// <summary>
        /// Inicia sesión del usuario especificado.
        /// </summary>
        /// <param name="user">El usuario que iniciará sesión.</param>
        /// <param name="isPersistent">Indica si la sesión es persistente o no.</param>
        /// <param name="authenticationMethod">Método de autenticación opcional.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null);

        /// <summary>
        /// Intenta iniciar sesión del usuario con contraseña especificada.
        /// </summary>
        /// <param name="user">El usuario que intenta iniciar sesión.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <param name="isPersistent">Indica si la sesión es persistente o no.</param>
        /// <param name="lockoutOnFailure">Indica si se debe bloquear el usuario después de varios intentos fallidos.</param>
        /// <returns>El resultado de la operación de inicio de sesión.</returns>
        Task<SignInResult> PasswordSignInAsync(string user, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// Cierra la sesión actual del usuario.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task SignOutAsync();

        /// <summary>
        /// Configura las propiedades de autenticación externa.
        /// </summary>
        /// <param name="provider">El proveedor de autenticación externa.</param>
        /// <param name="redirectUrl">La URL de redirección después de la autenticación externa.</param>
        /// <param name="userId">El ID de usuario opcional.</param>
        /// <returns>Las propiedades de autenticación configuradas.</returns>
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, [StringSyntax(StringSyntaxAttribute.Uri)] string? redirectUrl, string? userId = null);

        /// <summary>
        /// Obtiene la información de inicio de sesión externa.
        /// </summary>
        /// <param name="expectedXsrf">Token XSRF esperado opcional.</param>
        /// <returns>La información de inicio de sesión externa.</returns>
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null);

        /// <summary>
        /// Obtiene la información de inicio de sesión externa.
        /// </summary>
        /// <param name="expectedXsrf">Token XSRF esperado opcional.</param>
        /// <returns>La información de inicio de sesión externa.</returns>
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);

        /// <summary>
        /// Actualiza los tokens de autenticación externa.
        /// </summary>
        /// <param name="externalLogin">La información de inicio de sesión externa.</param>
        /// <returns>El resultado de la operación de actualización de tokens de autenticación externa.</returns>
        Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin);

        /// <summary>
        /// Obtiene el usuario para la autenticación de dos factores.
        /// </summary>
        /// <returns>El usuario para la autenticación de dos factores.</returns>
        Task<TUser> GetTwoFactorAuthenticationUserAsync();

        /// <summary>
        /// Inicia sesión con el autenticador de dos factores.
        /// </summary>
        /// <param name="code">El código de autenticación de dos factores.</param>
        /// <param name="isPersistent">Indica si la sesión es persistente o no.</param>
        /// <param name="rememberClient">Indica si se debe recordar el cliente.</param>
        /// <returns>El resultado de la operación de inicio de sesión.</returns>
        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient);
    }
}
