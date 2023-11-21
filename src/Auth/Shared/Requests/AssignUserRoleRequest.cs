using System;

namespace Hyperspan.Auth.Shared.Requests
{
    public class AssignUserRoleRequest<T> where T : IEquatable<T>
    {
        public T UserId { get; set; }
        public T RoleId { get; set; }
    }
}
