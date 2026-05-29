using DisputePortal.Application.Feature.Lookup.Persistence;
using DisputePortal.Application.Feature.Lookup.Results;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly DisputePortalDbContext ctx;

    public LookupRepository(DisputePortalDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<IReadOnlyList<DisputeReasonDto>> GetDisputeReasonsAsync(CancellationToken ct)
        => await ctx.DisputeReasons
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .Select(r => new DisputeReasonDto(r.Id, r.Name, r.Description))
            .ToListAsync(ct);

    public async Task<IReadOnlyList<TransactionCategoryDto>> GetTransactionCategoriesAsync(CancellationToken ct)
        => await ctx.TransactionCategories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new TransactionCategoryDto(c.Id, c.Name, c.Description))
            .ToListAsync(ct);
}
