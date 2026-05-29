using DisputePortal.Application.Domain.Models;

namespace DisputePortal.Application.Feature.User.Persistence;

public interface IUserRepository
{
    Task<AppUserModel?> GetByEmailAsync(string email, CancellationToken ct);
    Task<RefreshTokenModel?> GetRefreshTokenAsync(string tokenHash, CancellationToken ct);
    Task AddRefreshTokenAsync(RefreshTokenModel token, CancellationToken ct);
    Task RevokeRefreshTokenAsync(string tokenHash, CancellationToken ct);
}
