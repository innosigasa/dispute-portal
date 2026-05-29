using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Feature.Auth.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DisputePortal.Application.Feature.Auth.Service;

public class JwtTokenService
{
    private readonly JwtBearerSettings jwtBearerSettings;

    public JwtTokenService(JwtBearerSettings jwtBearerSettings)
    {
        this.jwtBearerSettings = jwtBearerSettings;
    }

    public string GenerateAccessToken(string userId, string role, Guid? customerId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        if (customerId.HasValue)
            claims.Add(new Claim(AppConstants.ClaimTypeCustomerId, customerId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtBearerSettings.Issuer,
            audience: jwtBearerSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtBearerSettings.AccessTokenExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string token, string hash, DateTime expiresAt) GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes);
        var hash = HashToken(token);
        var expiresAt = DateTime.UtcNow.AddDays(jwtBearerSettings.RefreshTokenExpiryDays);
        return (token, hash, expiresAt);
    }

    public static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
