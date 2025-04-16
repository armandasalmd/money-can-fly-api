using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using MCF.Core.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace MCF.Core.Auth;

public class FirebaseJwtVerifier(IHttpClientFactory httpClientFactory, IConfiguration configuration)
{
    public async Task<CookieUser> VerifyTokenAsync(string jwt)
    {
        var firebaseProjectId = configuration["Firebase:ProjectId"] ?? throw new NullReferenceException("ENV missing: FirebaseProjectId");
        var httpClient = httpClientFactory.CreateClient();
        
        // Fetch public keys
        var response = await httpClient.GetStringAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com");
        var keys = JObject.Parse(response);

        // Get the kid from the JWT header
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var kid = token.Header.Kid;

        // Find the matching public key
        if (!keys.TryGetValue(kid, out var certString)) 
            throw new CryptographicException("Missing JWT certificate (kid)");

        // Load the X.509 certificate using X509CertificateLoader
        var certBytes = System.Text.Encoding.UTF8.GetBytes(certString.ToString());
        var certificate = X509CertificateLoader.LoadCertificate(certBytes);
        var rsa = certificate.GetRSAPublicKey();

        // Configure validation parameters
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuerSigningKey = true
        };

        handler.ValidateToken(jwt, validationParameters, out _);
        return new CookieUser(token.Payload);
    }
}