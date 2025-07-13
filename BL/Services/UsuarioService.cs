using BL.IServices;
using DA.CentralContext;
using DA.IRepository;
using DA.IUOW;
using Entity.CentralModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepositoryCentral<PerfilUsuario> _genericRepository;
        private readonly IGenericRepositoryCentral<UvAspnetUserRole> _genericRepositoryPerfil;
        private readonly IUnitOfWorkCentral _unitOfWork;

        public UsuarioService(IGenericRepositoryCentral<PerfilUsuario> genericRepository, IGenericRepositoryCentral<UvAspnetUserRole> genericRepositoryPerfil, IUnitOfWorkCentral unitOfWork)
        {
            _genericRepository = genericRepository;
            _genericRepositoryPerfil = genericRepositoryPerfil;
            _unitOfWork = unitOfWork;
        }

        public Task<PerfilUsuario> Crear(PerfilUsuario entidad)
        {
            var repository = _unitOfWork.GetRepository<PerfilUsuario>();
            return repository.Crear(entidad);
        }

        public async Task<bool> Editar(PerfilUsuario entidad)
        {
            var repository = _unitOfWork.GetRepository<PerfilUsuario>();
            return await repository.Editar(entidad);
        }

        public PerfilUsuario Find(int id)
        {
            var repository = _unitOfWork.GetRepository<PerfilUsuario>();
            return repository.Encontrar(id);
        }

        public async Task<PerfilUsuario> FindAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<PerfilUsuario>();
            return await repository.EncontrarAsincrono(id);
        }

        public async Task<PerfilUsuario> ObtenerPorIdRef(string idRef)
        {
            var repository = _unitOfWork.GetRepository<PerfilUsuario>();
            return await repository.Obtener(p => p.RefAspNetUser == idRef);
        }

        public async Task<IQueryable<UvAspnetUserRole>> ListarPerfilUsuarios()
        {
            var repository = _unitOfWork.GetRepository<UvAspnetUserRole>();
            IQueryable<UvAspnetUserRole> query = await repository.Consultar();
            return query;
        }

        public async Task<UvAspnetUserRole?> ObtenerUserRoles(string id = null)
        {
            var repository = _unitOfWork.GetRepository<UvAspnetUserRole>();
            UvAspnetUserRole? output = await repository.Obtener(p => p.Id == id);
            return output;
        }

        public async Task<List<UsuariosV2>> ListarUsuariosV2(int minimo, int maximo)
        {
            var repository = _unitOfWork.GetRepository<UsuariosV2>();
            IQueryable<UsuariosV2> query = await repository.Consultar(u => u.SistemaId == 13 && u.RefHistoriaClinica != null && u.UsuarioId >= minimo && u.UsuarioId <= maximo);
            return query.ToList();
        }
    }
}
