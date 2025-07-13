using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BL.IServices
{
    /// <summary>
    /// Clase que proporciona servicios de gestión de usuarios, incluyendo operaciones como crear usuarios, gestionar roles, restablecer contraseñas, etc.
    /// </summary>
    public interface IUserManagerService<TUser> where TUser : class
    {
        /// <summary>
        /// Crea un nuevo usuario con la contraseña especificada.
        /// </summary>
        /// <param name="user">El usuario a crear.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>Una tarea que representa la operación de creación, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> CreateAsync(TUser user, string password);

        /// <summary>
        /// Crea un nuevo usuario con el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario a crear.</param>
        /// <returns>Una tarea que representa la operación de creación, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> CreateAsync(TUser user);

        /// <summary>
        /// Agrega un usuario al rol especificado.
        /// </summary>
        /// <param name="user">El usuario a agregar al rol.</param>
        /// <param name="role">El nombre del rol al que se agregará el usuario.</param>
        /// <returns>Una tarea que representa la operación de adición al rol, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);

        /// <summary>
        /// Genera un token de confirmación de correo electrónico para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se generará el token de confirmación de correo electrónico.</param>
        /// <returns>Una tarea que representa la operación de generación de token, que devuelve el token generado.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        /// <summary>
        /// Busca un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del usuario a buscar.</param>
        /// <returns>Una tarea que representa la operación de búsqueda, que devuelve el usuario encontrado o null si no se encuentra.</returns>
        Task<TUser?> FindByEmailAsync(string email);

        /// <summary>
        /// Genera un token para restablecer la contraseña del usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se generará el token de restablecimiento de contraseña.</param>
        /// <returns>Una tarea que representa la operación de generación de token, que devuelve el token generado.</returns>
        Task<string> GeneratePasswordResetTokenAsync(TUser user);

        /// <summary>
        /// Restablece la contraseña del usuario especificado utilizando un token y una nueva contraseña.
        /// </summary>
        /// <param name="user">El usuario cuya contraseña se restablecerá.</param>
        /// <param name="code">El token de restablecimiento de contraseña.</param>
        /// <param name="password">La nueva contraseña.</param>
        /// <returns>Una tarea que representa la operación de restablecimiento de contraseña, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> ResetPasswordAsync(TUser user, string code, string password);

        /// <summary>
        /// Busca un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a buscar.</param>
        /// <returns>Una tarea que representa la operación de búsqueda, que devuelve el usuario encontrado o null si no se encuentra.</returns>
        Task<TUser> FindByIdAsync(string id);

        /// <summary>
        /// Confirma el correo electrónico del usuario utilizando un token.
        /// </summary>
        /// <param name="user">El usuario cuyo correo electrónico se confirmará.</param>
        /// <param name="code">El token de confirmación de correo electrónico.</param>
        /// <returns>Una tarea que representa la operación de confirmación, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string code);

        /// <summary>
        /// Agrega un inicio de sesión externo asociado con un usuario.
        /// </summary>
        /// <param name="user">El usuario al que se asociará el inicio de sesión externo.</param>
        /// <param name="info">La información del inicio de sesión externo.</param>
        /// <returns>Una tarea que representa la operación de adición de inicio de sesión, que devuelve un resultado de identidad.</returns>
        Task<IdentityResult> AddLoginAsync(TUser user, ExternalLoginInfo info);

        /// <summary>
        /// Obtiene el usuario asociado con el principal de reclamaciones (claims) especificado.
        /// </summary>
        /// <param name="user">El principal de reclamaciones (claims) asociado con el usuario a obtener.</param>
        /// <returns>Una tarea que representa la operación de obtención del usuario, que devuelve el usuario asociado o null si no se encuentra.</returns>
        Task<TUser?> GetUserAsync(ClaimsPrincipal user);

        /// <summary>
        /// Restablece la clave del autenticador para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se restablecerá la clave del autenticador.</param>
        /// <returns>Una tarea que representa la operación de restablecimiento, que no devuelve ningún resultado.</returns>
        Task ResetAuthenticatorKeyAsync(TUser user);

        /// <summary>
        /// Obtiene la clave del autenticador para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se obtendrá la clave del autenticador.</param>
        /// <returns>Una tarea que representa la operación de obtención, que devuelve la clave del autenticador o null si no se encuentra.</returns>
        Task<string?> GetAuthenticatorKeyAsync(TUser user);

        /// <summary>
        /// Establece si la autenticación de dos factores está habilitada para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se establecerá la autenticación de dos factores.</param>
        /// <param name="enabled">Indica si se debe habilitar o deshabilitar la autenticación de dos factores.</param>
        /// <returns>Una tarea que representa la operación de configuración, que no devuelve ningún resultado.</returns>
        Task SetTwoFactorEnabledAsync(TUser user, Boolean enabled);

        /// <summary>
        /// Verifica el token de autenticación de dos factores para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se verificará el token de autenticación de dos factores.</param>
        /// <param name="tokenProvider">El proveedor del token de autenticación de dos factores.</param>
        /// <param name="token">El token de autenticación de dos factores.</param>
        /// <returns>Una tarea que representa la operación de verificación, que devuelve un valor booleano indicando si el token es válido o no.</returns>
        Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token);

        /// <summary>
        /// Actualiza la información del usuario en el sistema de identidad.
        /// </summary>
        /// <param name="user">El usuario cuya información debe actualizarse.</param>
        /// <returns>Una tarea que representa la operación de actualización.</returns>
        Task UpdateAsync(TUser user);

        /// <summary>
        /// Elimina al usuario del rol especificado en el sistema de identidad.
        /// </summary>
        /// <param name="user">El usuario del cual se eliminará el rol.</param>
        /// <param name="role">El nombre del rol que se eliminará del usuario.</param>
        /// <returns>Un objeto de tipo IdentityResult que representa el resultado de la operación de eliminación del rol.</returns>
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role);

        // Agrega otros métodos de UserManager que necesites utilizar en tu aplicación
        /// <summary>
        /// Lista el total de usuarios filtrados con una condicion.
        /// </summary>
        /// <param name="">El usuario cuya contraseña debe restablecerse.</param>
        /// <param name="">La nueva contraseña para el usuario.</param>
        /// <returns>Una lista de IdentityUser que representa el resultado de la operación de consulta.</returns>
        Task<List<TUser>> ListarUsuarios();

        /// <summary>
        /// Elimina al usuario en el sistema de identidad.
        /// </summary>
        /// <param name="user">El usuario el cual será eliminará.</param>
        /// <returns>Una tarea que representa la operación de configuración, que no devuelve ningún resultado.</returns>
        Task RemoveUser(IdentityUser user);

        /// <summary>
        /// Devuelve informacion del acceso extero del usuario.
        /// </summary>
        /// <param name="user">El usuario el cual será eliminará.</param>
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);
    }
}
