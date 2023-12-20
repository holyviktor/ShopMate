using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Controllers;

public class IdentityController : ControllerBase
{
    private const string TokenSecret = "JwtKeyShopMateForProgrammingInOurLifeIwantThisStore";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    [HttpPost("token")]
    public IActionResult GenerateToken(
        [FromBody] TokenGenerationRequest request)
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
       
        // foreach (var claimPair in request.CustomClaims)
        // {
        //     var jsonElement = (JsonElement)claimPair.Value;
        //     var valueType = jsonElement.ValueKind switch
        //     {
        //         JsonValueKind.True => ClaimValueTypes.Boolean,
        //         JsonValueKind.False => ClaimValueTypes.Boolean,
        //         JsonValueKind.Number => ClaimValueTypes.Double,
        //         _ => ClaimValueTypes.String
        //     };
        //
        //     var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
        //     claims.Add(claim);
        // }
        
        

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = "https://localhost:7078",
            Audience = "https://localhost:7078",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);

    }

}