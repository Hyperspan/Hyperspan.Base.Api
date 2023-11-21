using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Hyperspan.Auth.Interfaces;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Http;

namespace Hyperspan.Auth.Services
{
    public abstract class CurrentUserService<T> : ICurrentUserService<T> where T : IEquatable<T>, IBaseEntity<T>
    {

        /// <summary>
        /// Get Details of current LoggedIn User
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        protected CurrentUserService(IHttpContextAccessor httpContextAccessor)
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

        public abstract T UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string>? UserRoles { get; set; }

        public List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
