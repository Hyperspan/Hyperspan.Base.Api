using System.ComponentModel.DataAnnotations;
using Shared.Enums.Users;

namespace Shared.Responses.Users;

public class RegisterResponse
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public RegistrationStages RegistrationStage { get; set; }
}
