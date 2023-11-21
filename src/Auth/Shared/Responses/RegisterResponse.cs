using System.ComponentModel.DataAnnotations;
using Hyperspan.Auth.Shared.Enums;

namespace Hyperspan.Auth.Shared.Responses;

public class RegisterResponse
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public RegistrationStages RegistrationStage { get; set; }
}
