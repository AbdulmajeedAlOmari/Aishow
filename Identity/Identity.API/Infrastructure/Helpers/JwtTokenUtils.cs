using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.API.Infrastructure.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Infrastructure.Helpers;

public class JwtTokenUtils
{
    public static string GenerateJwtToken(User user, string secret)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        // Create claims
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim("UserId", user.UserId.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        foreach (var userRole in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.CodeName));
        }

        // Generate token with claims
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // Return token as string/text
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}