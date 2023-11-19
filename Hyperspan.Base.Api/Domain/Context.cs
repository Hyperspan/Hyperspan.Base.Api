using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Base.Api.Domain
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}