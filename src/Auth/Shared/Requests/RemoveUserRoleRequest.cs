namespace Hyperspan.Auth.Shared.Requests
{
    public class RemoveUserRoleRequest<T>
    {
        public T UserId { get; set; }
        public T RoleId { get; set; }
    }
}
