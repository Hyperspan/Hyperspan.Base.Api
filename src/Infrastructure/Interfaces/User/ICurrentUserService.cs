namespace Infrastructure.Interfaces.User
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<string>? UserRoles { get; set; }

        public List<KeyValuePair<string, string>>? Claims { get; set; }

    }
}
