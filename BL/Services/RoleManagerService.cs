using BL.IServices;
using DA.IRepository;
using DA.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class RoleManagerService<TRole> : IRoleManagerService<TRole> where TRole : class
    {
        private readonly IRoleRepository<TRole> _roleRepository;
        private readonly IGenericRepositoryCentral<TRole> _genericRepository;

        public RoleManagerService(IRoleRepository<TRole> roleRepository, IGenericRepositoryCentral<TRole> genericRepository)
        {
            _roleRepository = roleRepository;
            _genericRepository = genericRepository;
        }

        public Task<IdentityResult> CreateAsync(TRole role)
        {
            return _roleRepository.CreateAsync(role);
        }

        public async Task<IQueryable<TRole>> ListarRoles()
        {
            IQueryable<TRole> query = await _genericRepository.Consultar();
            return query;
        }

        public Task<bool> RoleExistsAsync(string roneName)
        {
            return _roleRepository.RoleExistsAsync(roneName);
        }
    }
}
