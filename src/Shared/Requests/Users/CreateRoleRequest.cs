using System.ComponentModel.DataAnnotations;

namespace Shared.Requests.Users
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;

    }
}
