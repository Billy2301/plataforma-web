using DA.CentralContext;
using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IUsuarioService
    {
        Task<PerfilUsuario> FindAsync(int id);
        PerfilUsuario Find(int entidad);
        Task<bool> Editar(PerfilUsuario entidad);
        Task<PerfilUsuario> ObtenerPorIdRef(string idRef);
        Task<PerfilUsuario> Crear(PerfilUsuario entidad);
        Task<IQueryable<UvAspnetUserRole>> ListarPerfilUsuarios();
        Task<UvAspnetUserRole?> ObtenerUserRoles(string id = null);
        Task<List<UsuariosV2>> ListarUsuariosV2(int minimo, int maximo);
    }
}
