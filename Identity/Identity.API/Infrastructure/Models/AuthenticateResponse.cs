using Identity.API.Infrastructure.Entities;

namespace Identity.API.Infrastructure.Models;

public class AuthenticateResponse
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        UserId = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Token = token;
    }
}