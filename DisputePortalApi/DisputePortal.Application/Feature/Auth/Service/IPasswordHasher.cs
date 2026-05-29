namespace DisputePortal.Application.Feature.Auth.Service;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hash);
}
