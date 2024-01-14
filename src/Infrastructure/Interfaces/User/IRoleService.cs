using Domain.Entities;
using Shared.Modals;
using Shared.Requests.Users;

namespace Infrastructure.Interfaces.User
{
    public interface IRoleService
    {
        Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request);
        Task<ApiResponseModal<List<ApplicationRole<Guid>>>> ListAllRolesAsync();
        Task<ApiResponseModal<ApplicationRole<Guid>>> AssignUserRole(AssignUserRoleRequest<Guid> request);
        Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<Guid> request);
        Task<ApiResponseModal<List<ApplicationRole<Guid>>>> GetUserRoles(Guid userId);
    }
}