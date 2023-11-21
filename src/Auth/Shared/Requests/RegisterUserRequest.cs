using System.ComponentModel.DataAnnotations;

namespace Hyperspan.Auth.Shared.Requests;

public class RegisterUserRequest
{
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [EmailAddress, Required]
    public string Email { get; set; } = string.Empty;

    [MaxLength(15), Required]
    public string MobileNumber { get; set; } = string.Empty;

    [Compare("ConfirmPassword"), Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}

