using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Auth.Services;
using Hyperspan.Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hyperspan.Base.Services
{
    public class RoleService : RoleService<Guid>, IRoleService
    {
        public RoleService(
            RoleManager<ApplicationRole<Guid>> roleManager,
            UserManager<ApplicationUser<Guid>> userManager
            )
            : base(roleManager, userManager)
        {
        }
    }
}
