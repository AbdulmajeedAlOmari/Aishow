using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Data;

public class IdentityDataContext : DbContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}