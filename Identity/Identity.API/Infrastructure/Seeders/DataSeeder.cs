using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Seeders;

public static class DataSeeder
{
    public static void Seed(IdentityDataContext context)
    {
        SeedRoles(context);
    }

    private static void SeedRoles(IdentityDataContext context)
    {
        if (!context.Roles.Any())
        {
            context.AddRange(Roles.All);
            context.SaveChanges();
        }
    }

}