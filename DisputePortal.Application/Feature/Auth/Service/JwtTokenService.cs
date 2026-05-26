using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DisputePortal.Application.Feature.Auth.Service;

public class JwtTokenService(IConfiguration config)
{
    private string SecretKey => config["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT secret key not configured.");
    private string Issuer => config["Jwt:Issuer"] ?? "DisputePortalApi";
    private string Audience => config["Jwt:Audience"] ?? "DisputePortalClients";
    private int AccessTokenExpiryMinutes => int.Parse(config["Jwt:AccessTokenExpiryMinutes"] ?? "60");
    private int RefreshTokenExpiryDays => int.Parse(config["Jwt:RefreshTokenExpiryDays"] ?? "7");

    public string GenerateAccessToken(string userId, string role, Guid? customerId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        if (customerId.HasValue)
            claims.Add(new Claim("customerId", customerId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AccessTokenExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string token, string hash, DateTime expiresAt) GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes);
        var hash = HashToken(token);
        var expiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);
        return (token, hash, expiresAt);
    }

    public static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
