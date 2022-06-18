using System.ComponentModel.DataAnnotations;

namespace Identity.API.Infrastructure.Models;

public class RegisterRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; }

}