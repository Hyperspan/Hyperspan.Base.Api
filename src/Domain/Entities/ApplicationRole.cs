using Microsoft.AspNetCore.Identity;
using Shared.Config;

namespace Domain.Entities
{
    public class ApplicationRole<T> : IdentityRole<T>, IBaseEntity<T> where T : IEquatable<T>
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
