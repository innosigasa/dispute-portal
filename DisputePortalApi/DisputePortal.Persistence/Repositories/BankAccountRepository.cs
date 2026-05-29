using DisputePortal.Application.Feature.Account.Persistence;
using DisputePortal.Application.Feature.Account.Results;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly DisputePortalDbContext ctx;

    public BankAccountRepository(DisputePortalDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<IReadOnlyList<BankAccountDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct)
    {
        return await ctx.BankAccounts
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AccountTypeId)
            .Select(a => new BankAccountDto(
                a.Id, a.AccountNumber, a.AccountTypeId.ToString(),
                a.AccountName, a.Balance, a.Currency, a.IsDefault
            ))
            .ToListAsync(ct);
    }

    public async Task<BankAccountDto?> GetByIdAsync(Guid id, Guid customerId, CancellationToken ct)
    {
        return await ctx.BankAccounts
            .AsNoTracking()
            .Where(a => a.Id == id && a.CustomerId == customerId)
            .Select(a => new BankAccountDto(
                a.Id, a.AccountNumber, a.AccountTypeId.ToString(),
                a.AccountName, a.Balance, a.Currency, a.IsDefault
            ))
            .FirstOrDefaultAsync(ct);
    }
}
