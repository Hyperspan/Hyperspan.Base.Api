namespace Shared.Config
{
    public class BaseEntity<T> : IBaseEntity<T> where T : IEquatable<T>
    {
        public T Id { get; set; } = default!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Guid? DeletedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }

    public interface IBaseEntity<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
