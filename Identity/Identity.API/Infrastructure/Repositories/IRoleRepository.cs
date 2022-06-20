using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetRole(int roleId);
}

public class RoleRepository : IRoleRepository
{
    private readonly IdentityDataContext _context;

    public RoleRepository(IdentityDataContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRole(int roleId)
    {
        return await _context.Roles.Where(role => role!.RoleId == roleId).FirstOrDefaultAsync();
    }
}