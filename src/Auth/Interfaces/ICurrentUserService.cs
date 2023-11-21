using System;
using System.Collections.Generic;

namespace Hyperspan.Auth.Interfaces
{
    public interface ICurrentUserService<T> where T : IEquatable<T>
    {
        public T UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<string>? UserRoles { get; set; }

        public List<KeyValuePair<string, string>>? Claims { get; set; }

    }
}
