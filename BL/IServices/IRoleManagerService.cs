using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IRoleManagerService<TRole> where TRole : class
    {
        /// <summary>
        /// Comprueba si un rol con el nombre especificado existe en el sistema.
        /// </summary>
        /// <param name="roleName">Nombre del rol a verificar.</param>
        /// <returns>
        /// True si el rol existe, de lo contrario False.
        /// </returns>
        Task<Boolean> RoleExistsAsync(string roneName);

        /// <summary>
        /// Crea un nuevo rol con el nombre especificado.
        /// </summary>
        /// <param name="roleName">Nombre del rol a crear.</param>
        /// <returns>
        /// Un objeto <see cref="IdentityResult"/> que representa el resultado de la operación de creación.
        /// </returns>
        Task<IdentityResult> CreateAsync(TRole role);

        /// <summary>
        /// Lista el total de los roles guardados en la BD.
        /// </summary>
        /// <param name="roleName">Nombre del rol a crear.</param>
        /// <returns>
        /// Un objeto <see cref="IdentityRole"/> que representa el resultado de la operación de creación.
        /// </returns>
        Task<IQueryable<TRole>> ListarRoles();
    }
}
