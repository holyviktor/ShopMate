using System.Security.Claims;

namespace Identity.Api;

public class TokenGenerationRequest
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}