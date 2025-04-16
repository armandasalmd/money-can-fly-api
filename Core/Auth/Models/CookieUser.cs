using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MCF.Core.Auth.Models;

public sealed class CookieUser
{
    private const int SessionMaxAge = 259200; // 3 days in seconds

    public string UserUid { get; }
    public string Email { get; }
    public string Exp { get; }

    public CookieUser(JwtPayload jwtPayload)
    {
        UserUid = jwtPayload.Sub ?? throw new ArgumentException("User ID is missing in JWT payload.");
        Email = jwtPayload.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? throw new ArgumentException("Email is missing in JWT payload.");
        Exp = DateTime.UtcNow
            .AddSeconds(SessionMaxAge - 60)
            .ToString("O"); // ISO 8601 format (e.g., "2025-04-16T12:34:56.789Z")
    }
    public IEnumerable<Claim> AsClaims()
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Email, Email),
            new Claim(JwtRegisteredClaimNames.Exp, Exp),
            new Claim(JwtRegisteredClaimNames.Sub, UserUid),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        ];
    }
}