using System;
using System.Linq;
using Hyperspan.Auth.Domain.DatabaseModals;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Auth.Domain.Context
{
    /// <summary>
    /// An abstract class to register all the identity tables required.
    /// </summary>
    /// <typeparam name="T">Id Field of for all the tables present in Identity Framework. </typeparam>
    public abstract class AuthContext<T> : IdentityDbContext<ApplicationUser<T>, ApplicationRole<T>, T,
                            IdentityUserClaim<T>, IdentityUserRole<T>, IdentityUserLogin<T>, IdentityRoleClaim<T>,
                                    IdentityUserToken<T>>
                        where T : IEquatable<T>
    {
        /// <summary>
        /// Constructor to initialize the database context.
        /// </summary>
        /// <param name="options"></param>
        protected AuthContext(DbContextOptions options)
            : base(options)
        { }

        /// <summary>
        /// Override the model creating method to configure and modify tables
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole<T>>(entity =>
            {
                entity.ToTable("Roles", schema: "Account");
            });

            builder.Entity<ApplicationUser<T>>(entity =>
            {
                entity.ToTable("Users", schema: "Account");
            });

            builder.Entity<IdentityUserClaim<T>>(entity =>
            {
                entity.ToTable("UserClaims", schema: "Account");
            });

            builder.Entity<IdentityRoleClaim<T>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: "Account");
            });

            builder.Entity<IdentityUserRole<T>>(entity =>
            {
                entity.ToTable("UserRoles", schema: "Account");
            });

            builder.Entity<IdentityUserToken<T>>(entity =>
            {
                entity.ToTable("UserTokens", schema: "Account");
            });

            builder.Entity<IdentityUserLogin<T>>(entity =>
            {
                entity.ToTable("UserLogins", schema: "Account");
            });

        }

        /// <summary>
        /// Dispose all the members for this context
        /// </summary>
        ~AuthContext()
        {
            Dispose();
        }
    }
}
