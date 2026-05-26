using System.Security.Cryptography;
using System.Text;

namespace DisputePortal.Application.Feature.Auth.Service;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password + "dispute-portal-salt"));
        return Convert.ToBase64String(bytes);
    }

    public bool Verify(string password, string hash) => Hash(password) == hash;
}
