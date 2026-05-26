using DisputePortal.Application.Domain.Entities;

namespace DisputePortal.Application.Feature.User.Persistence;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct);
    Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken ct);
    Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct);
    Task RevokeRefreshTokenAsync(string tokenHash, CancellationToken ct);
}
