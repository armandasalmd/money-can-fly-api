using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MCF.Core.Auth;
using MCF.Core.Auth.Models;
using MCF.Identity.Client.Models.Request;
using MCF.Identity.Client.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MCF.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(FirebaseJwtVerifier firebaseJwtVerifier, IConfiguration configuration) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.UserIdToken))
        {
            return LoginResponse.Failed("ID token is required");
        }
        
        try
        {
            // Step 1. Verify userIdToken (Firebase)
            var user = await firebaseJwtVerifier.VerifyTokenAsync(request.UserIdToken);
            
            // Step 2. Create new JWT session and store in a cookie
            var sessionJwt = GenerateSessionJwt(user);

            Response.Cookies.Append(Constants.SessionCookieName, sessionJwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddSeconds(Constants.SessionMaxAge - 1000)
            });

            return new LoginResponse
            {
                Success = true,
                User = user,
                Message = "Cookie created successfully"
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Token verification failed: {ex.Message}",
            };
        }
    }
    
    [HttpGet("logout")]
    public LogoutResponse Logout()
    {
        Response.Cookies.Delete(Constants.SessionCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
        
        return new LogoutResponse()
        {
            Success = true,
            Timestamp = DateTime.UtcNow,
        };
    }
    
    private string GenerateSessionJwt(CookieUser user)
    {
        var jwtSecret = configuration["SessionJwt:Secret"] ?? throw new NullReferenceException("ENV missing: SessionJwt:Secret");
        var issuer = configuration["SessionJwt:Issuer"] ?? throw new NullReferenceException("ENV missing: SessionJwt:Issuer");
        var audience = configuration["SessionJwt:Audience"] ?? throw new NullReferenceException("ENV missing: SessionJwt:Audience");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: user.AsClaims(),
            expires: DateTime.UtcNow.AddSeconds(Constants.SessionMaxAge),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}