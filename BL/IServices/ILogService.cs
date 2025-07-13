using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface ILogService
    {
        Task SaveLog(string Nivel, string modulo, string clase, string metodo, string Browser, string UsuarioId, string UsuarioNombre, string UsuarioIp, string mensaje, string InformacionAdicional);

        Task<IQueryable<PortalUsuarioLog>> ListarLogUsuario(string userName);
    }
}
