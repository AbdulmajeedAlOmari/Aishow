using System.ComponentModel.DataAnnotations;

namespace Identity.API.Infrastructure.Models;

public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}