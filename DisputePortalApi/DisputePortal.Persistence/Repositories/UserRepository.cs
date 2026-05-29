using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Feature.User.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DisputePortalDbContext ctx;

    public UserRepository(DisputePortalDbContext ctx)
    {
        this.ctx = ctx;
    }

    public Task<AppUserModel?> GetByEmailAsync(string email, CancellationToken ct)
        => ctx.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<RefreshTokenModel?> GetRefreshTokenAsync(string tokenHash, CancellationToken ct)
        => ctx.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.TokenHash == tokenHash && r.RevokedAt == null && r.ExpiresAt > DateTime.UtcNow, ct);

    public async Task AddRefreshTokenAsync(RefreshTokenModel token, CancellationToken ct)
    {
        ctx.RefreshTokens.Add(token);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task RevokeRefreshTokenAsync(string tokenHash, CancellationToken ct)
    {
        await ctx.RefreshTokens
            .Where(r => r.TokenHash == tokenHash)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.RevokedAt, DateTime.UtcNow), ct);
    }
}
