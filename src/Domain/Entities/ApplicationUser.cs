using Microsoft.AspNetCore.Identity;
using Shared.Config;
using Shared.Enums.Users;

namespace Domain.Entities;

public class ApplicationUser<T> : IdentityUser<T>, IBaseEntity<T> where T : IEquatable<T>
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public RegistrationStages RegistrationStage { get; set; } = RegistrationStages.None;

}