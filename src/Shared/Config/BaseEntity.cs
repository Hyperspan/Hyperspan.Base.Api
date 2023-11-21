namespace Hyperspan.Shared.Config
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;
    }

    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
