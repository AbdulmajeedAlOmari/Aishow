using Identity.API.Infrastructure.Entities;
using Identity.API.Infrastructure.Entities.Common;
using Identity.API.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Data;

public class IdentityDataContext : DbContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // ===[ Table Constraints ]===
        // Users Table
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Roles Table
        builder.Entity<Role>()
            .HasIndex(r => r.CodeName)
            .IsUnique();

        // ===[ Soft Delete Configurations ]===
        // Apply soft delete to all tables
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            //other automated configurations left out
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }
        }
    }
}