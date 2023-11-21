using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Auth.Interfaces;
using Hyperspan.Auth.Shared.Requests;
using Hyperspan.Shared;
using Hyperspan.Shared.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Auth.Services;

public abstract class RoleService<T> : IRoleService<T> where T : IEquatable<T>
{
    private readonly RoleManager<ApplicationRole<T>> _roleManager;
    private readonly UserManager<ApplicationUser<T>> _userManager;

    protected RoleService(RoleManager<ApplicationRole<T>> roleManager, UserManager<ApplicationUser<T>> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="request">Role details</param>
    /// <returns>ApiResponse stating whether the Operation was successful</returns>
    public virtual async Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request)
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
            var role = new ApplicationRole<T>
            {
                Name = request.RoleName,
            };

            var roleData = await _roleManager.CreateAsync(role);
            if (!roleData.Succeeded)
            {
                throw new ApiErrorException(string.Join('|', roleData.Errors.Select(x => x.Code + x.Description).ToList()));
            }

            return await ApiResponseModal.SuccessAsync();
        }
        catch (ApiErrorException e)
        {
            return await ApiResponseModal.FatalAsync(e, BaseErrorCodes.UnknownSystemException);
        }
    }

    /// <summary>
    /// List All the available roles.
    /// </summary>
    /// <returns>Roles available in the database</returns>
    public virtual async Task<ApiResponseModal<List<ApplicationRole<T>>>> ListAllRolesAsync()
    {
        try
        {
            // Fetch all roles
            var roles = await _roleManager.Roles.ToListAsync();

            // Map to an object
            return await ApiResponseModal<List<ApplicationRole<T>>>.SuccessAsync(roles);
        }
        catch (ApiErrorException e)
        {
            return await ApiResponseModal<List<ApplicationRole<T>>>.FatalAsync(e,
                BaseErrorCodes.UnknownSystemException);
        }
    }

    /// <summary>
    /// Assign particular Role to the user
    /// </summary>
    /// <param name="request">UserId and RoleId</param>
    /// <returns></returns>
    public virtual async Task<ApiResponseModal<ApplicationRole<T>>> AssignUserRole(AssignUserRoleRequest<T> request)
    {
        // Find Role
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        // Find User
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        // Add Role to user
        var result = await _userManager.AddToRoleAsync(user, role.Name);

        if (result is not { Succeeded: true })
            return await ApiResponseModal<ApplicationRole<T>>.FailedAsync(BaseErrorCodes.UnknownSystemException);

        return await ApiResponseModal<ApplicationRole<T>>.SuccessAsync(role);

    }

    /// <summary>
    /// Get all the roles assigned to the user
    /// </summary>
    /// <param name="userId">The Primary key of the user</param>
    /// <returns>A List of all the roles of that user</returns>
    public virtual async Task<ApiResponseModal<List<ApplicationRole<T>>>> GetUserRoles(T userId)
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
                return await ApiResponseModal<List<ApplicationRole<T>>>.FailedAsync(BaseErrorCodes.UnknownSystemException);
            }

            // Init a new role list
            var roleList = new List<ApplicationRole<T>>();
            foreach (var item in userRoles)
            {
                // Get the role object and append to the list
                var role = await _roleManager.FindByNameAsync(item);
                roleList.Add(role);
            }

            // return all the roles
            return await ApiResponseModal<List<ApplicationRole<T>>>.SuccessAsync(roleList);
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
    public virtual async Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<T> request)
    {
        // Find User
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        // Find Role
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        // Remove user from the Role
        var userRole = await _userManager.RemoveFromRoleAsync(user, role.Name);

        // Throw/ return errors 
        if (userRole is not { Succeeded: true })
            return await ApiResponseModal.FailedAsync(BaseErrorCodes.UnknownSystemException);

        return await ApiResponseModal.SuccessAsync();
    }
}