using BL.IServices;
using DA.IUOW;
using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;

        public LogService(IUnitOfWorkCentral unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveLog(string? Nivel,string? modulo, string? clase, string? metodo, string? Browser, string? UsuarioId, string? UsuarioNombre, string? UsuarioIp, string? mensaje, string? InformacionAdicional)
        {
            PortalUsuarioLog model = new PortalUsuarioLog();
            model.Nivel = Nivel;
            model.Modulo = modulo;
            model.Clase = clase;
            model.Metodo = metodo;
            model.Browser = Browser;
            model.UsuarioId = UsuarioId;
            model.UsuarioNombre = UsuarioNombre;
            model.UsuarioIp = UsuarioIp;
            model.FechaHora = DateTime.Now;
            model.Mensaje = mensaje;
            model.InformacionAdicional = InformacionAdicional;

            await Guardar(model);
        }

        public async Task Guardar(PortalUsuarioLog entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<PortalUsuarioLog>();
                PortalUsuarioLog entity_created = await repository.Crear(entity);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<IQueryable<PortalUsuarioLog>> ListarLogUsuario(string userName)
        {
            var repository = _unitOfWork.GetRepository<PortalUsuarioLog>();
            IQueryable<PortalUsuarioLog> query = await repository.Consultar(u => u.UsuarioNombre == userName);
            return query;
        }

    }
}
