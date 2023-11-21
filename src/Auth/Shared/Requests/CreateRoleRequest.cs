using System.ComponentModel.DataAnnotations;

namespace Hyperspan.Auth.Shared.Requests
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;

    }
}
