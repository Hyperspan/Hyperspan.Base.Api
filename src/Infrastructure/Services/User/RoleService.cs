using Domain.Entities;
using Infrastructure.Interfaces.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Modals;
using Shared;
using Shared.Requests.Users;

namespace Infrastructure.Services.User
{
    public sealed class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser<Guid>> _userManager;
        private readonly ILogger _logger;

        public RoleService(
            RoleManager<ApplicationRole<Guid>> roleManager,
            UserManager<ApplicationUser<Guid>> userManager,
            ILogger logger
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="request">Role details</param>
        /// <returns>ApiResponse stating whether the Operation was successful</returns>
        public async Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request)
        {
            try
            {
                // Check role's name is not null or string.Empty
                if (string.IsNullOrEmpty(request.RoleName)) throw new ApiErrorException(BaseErrorCodes.NullValue);

                // Check for existing role
                var existingRole = await _roleManager.FindByNameAsync(request.RoleName);
                if (existingRole != null)
                {
                    throw new ApiErrorException(BaseErrorCodes.RoleExists);
                }

                // Create new role for requested user name
                var role = new ApplicationRole<Guid>
                {
                    Name = request.RoleName,
                };

                var roleData = await _roleManager.CreateAsync(role);
                if (!roleData.Succeeded)
                {
                    throw new ApiErrorException(string.Join('|',
                        roleData.Errors.Select(x => x.Code + x.Description).ToList()));
                }

                return await ApiResponseModal.SuccessAsync();
            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal.FatalAsync(e, BaseErrorCodes.UnknownSystemException, _logger);
            }
        }

        /// <summary>
        /// List All the available roles.
        /// </summary>
        /// <returns>Roles available in the database</returns>
        public async Task<ApiResponseModal<List<ApplicationRole<Guid>>>> ListAllRolesAsync()
        {
            try
            {
                // Fetch all roles
                var roles = await _roleManager.Roles.ToListAsync();

                // Map to an object
                return await ApiResponseModal<List<ApplicationRole<Guid>>>.SuccessAsync(roles);
            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal<List<ApplicationRole<Guid>>>.FatalAsync(e,
                    BaseErrorCodes.UnknownSystemException, _logger);
            }
        }

        /// <summary>
        /// Assign particular Role to the user
        /// </summary>
        /// <param name="request">UserId and RoleId</param>
        /// <returns></returns>
        public async Task<ApiResponseModal<ApplicationRole<Guid>>> AssignUserRole(
            AssignUserRoleRequest<Guid> request)
        {
            // Find Role
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            // Find User
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            // Add Role to user
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result is not { Succeeded: true })
                return await ApiResponseModal<ApplicationRole<Guid>>.FailedAsync(BaseErrorCodes.UnknownSystemException,
                    _logger);

            return await ApiResponseModal<ApplicationRole<Guid>>.SuccessAsync(role);
        }

        /// <summary>
        /// Get all the roles assigned to the user
        /// </summary>
        /// <param name="userId">The Primary key of the user</param>
        /// <returns>A List of all the roles of that user</returns>
        public async Task<ApiResponseModal<List<ApplicationRole<Guid>>>> GetUserRoles(Guid userId)
        {
            try
            {
                // Find user
                var user = await _userManager.FindByIdAsync(userId.ToString());
                // Get User's Roles
                var userRoles = await _userManager.GetRolesAsync(user);

                // If user roles are null return unknown error
                if (userRoles == null)
                {
                    return await ApiResponseModal<List<ApplicationRole<Guid>>>.FailedAsync(
                        BaseErrorCodes.UnknownSystemException, _logger);
                }

                // Init a new role list
                var roleList = new List<ApplicationRole<Guid>>();
                foreach (var item in userRoles)
                {
                    // Get the role object and append to the list
                    var role = await _roleManager.FindByNameAsync(item);
                    roleList.Add(role);
                }

                // return all the roles
                return await ApiResponseModal<List<ApplicationRole<Guid>>>.SuccessAsync(roleList);
            }
            catch (ApiErrorException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Remove the role assigned to an user
        /// </summary>
        /// <param name="request">Request Object containing the userid and role id</param>
        /// <returns>ApiResponseModal</returns>
        public async Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<Guid> request)
        {
            // Find User
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            // Find Role
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            // Remove user from the Role
            var userRole = await _userManager.RemoveFromRoleAsync(user, role.Name);

            // Throw/ return errors 
            if (userRole is not { Succeeded: true })
                return await ApiResponseModal.FailedAsync(BaseErrorCodes.UnknownSystemException, _logger);

            return await ApiResponseModal.SuccessAsync();
        }
    }
}