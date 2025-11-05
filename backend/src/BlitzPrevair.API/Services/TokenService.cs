using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlitzPrevair.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BlitzPrevair.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly int _tokenExpirationHours = 24;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Customer customer)
    {
        var jwtSecret = _configuration["Jwt:Secret"] ?? "AppointmentApp-Super-Secret-Key-Change-In-Production-Min-32-Chars";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "AppointmentApp";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "AppointmentApp";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, customer.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, customer.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("customerId", customer.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_tokenExpirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetTokenExpiration()
    {
        return DateTime.UtcNow.AddHours(_tokenExpirationHours);
    }
}
