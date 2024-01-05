using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EzTech.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace EzTech.Api.Authentication;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static UserPrincipal GetUser(ClaimsPrincipal claims)
    {
        var user = new UserPrincipal
        {
            Id = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            FirstName = claims.FindFirst(ClaimTypes.GivenName)!.Value,
            Role = claims.FindFirst(ClaimTypes.Role)!.Value,
        };
        return user;
    }

    public string GenerateToken(User user)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()!),
            new(ClaimTypes.GivenName, user.FirstName!),
            new(ClaimTypes.Surname, user.LastName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role, user.Role.ToString()),
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = "EzTech.dk",
            Audience = "EzTech.dk",
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(authClaims)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}