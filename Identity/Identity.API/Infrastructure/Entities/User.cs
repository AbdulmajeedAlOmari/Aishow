using System.ComponentModel.DataAnnotations;

namespace Identity.API.Infrastructure.Entities;

public class User
{
    public int UserId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public ICollection<Role> Roles { get; set; }

    public User()
    {
        Roles = new HashSet<Role>();
    }
}