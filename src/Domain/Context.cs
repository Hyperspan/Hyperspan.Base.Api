using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class Context : AuthContext<Guid>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
    }
}