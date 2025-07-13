using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.IRepository
{
    public interface IRoleRepository<TRole> where TRole : class
    {
        Task<Boolean> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateAsync(TRole role);
    }
}
