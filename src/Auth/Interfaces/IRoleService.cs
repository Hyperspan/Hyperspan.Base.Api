using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Auth.Shared.Requests;
using Hyperspan.Shared.Modals;

namespace Hyperspan.Auth.Interfaces
{
    public interface IRoleService<T> where T : IEquatable<T>
    {
        Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request);
        Task<ApiResponseModal<List<ApplicationRole<T>>>> ListAllRolesAsync();
        Task<ApiResponseModal<ApplicationRole<T>>> AssignUserRole(AssignUserRoleRequest<T> request);
        Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<T> request);
        Task<ApiResponseModal<List<ApplicationRole<T>>>> GetUserRoles(T userId);
    }
}
