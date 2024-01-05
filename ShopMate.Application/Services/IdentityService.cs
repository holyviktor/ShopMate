using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShopMate.Core.Models;

namespace ShopMate.Application.Services;

public class IdentityService
{
    private const string TokenSecret = "JwtKeyShopMateForProgrammingInOurLifeIwantThisStore";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(24*30);
    
    public String GenerateToken(TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId),
            new( "user_role", request.Role)
        };


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = "https://localhost:7019",
            Audience = "https://localhost:7019",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}