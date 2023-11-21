using System;
using Hyperspan.Auth.Shared.Enums;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Hyperspan.Auth.Domain.DatabaseModals;

public class ApplicationUser<T> : IdentityUser<T>, IBaseEntity<T> where T : IEquatable<T>
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;
    public RegistrationStages RegistrationStage { get; set; } = RegistrationStages.None;

}