using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<User?> GetUser(int userId);
    Task<User?> GetUserWithRolesByUsername(string username);
    Task<List<User>> GetUsers();
}

public class UserRepository : IUserRepository
{
    private readonly IdentityDataContext _context;

    public UserRepository(IdentityDataContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task<User?> GetUser(int userId)
    {
        return await _context.Users.Where(user => user!.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithRolesByUsername(string username)
    {
        return await _context.Users
            .Include(x => x.Roles)
            .Where(user => user!.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}