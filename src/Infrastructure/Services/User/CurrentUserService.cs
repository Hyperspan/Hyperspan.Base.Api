using System.Security.Claims;
using Infrastructure.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Shared.Config;

namespace Infrastructure.Services.User
{
    public class CurrentUserService : ICurrentUserService
    {

        /// <summary>
        /// Get Details of current LoggedIn User
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var claimsPrincipal = httpContextAccessor.HttpContext?.User;

            if (claimsPrincipal != null)
            {

                UserName = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
                Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            }

            // Get All Claims
            Claims = claimsPrincipal?.Claims.AsEnumerable().Select(item =>
                    new KeyValuePair<string, string>(item.Type, item.Value))
                .ToList();

            // Get All Roles
            UserRoles = Claims?.Where(c => c.Key == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string>? UserRoles { get; set; }

        public List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
