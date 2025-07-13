using DA.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repository
{
    public class RoleRepository<TRole> : IRoleRepository<TRole> where TRole : class
    {
        private readonly RoleManager<TRole> _roleManager;
        private readonly RoleManager<IdentityRole> _roleManagers;

        public RoleRepository(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<IdentityResult> CreateAsync(TRole role)
        {
            return _roleManager.CreateAsync(role);
        }

        public Task<bool> RoleExistsAsync(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName);
        }
    }
}
