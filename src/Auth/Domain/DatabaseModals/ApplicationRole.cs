using System;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Hyperspan.Auth.Domain.DatabaseModals
{
    public class ApplicationRole<T> : IdentityRole<T>, IBaseEntity<T> where T : IEquatable<T>
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;
    }
}
