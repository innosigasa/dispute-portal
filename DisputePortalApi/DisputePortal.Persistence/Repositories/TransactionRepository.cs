using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Account.Results;
using DisputePortal.Application.Feature.Transaction.Persistence;
using DisputePortal.Application.Feature.Transaction.Requests;
using DisputePortal.Application.Feature.Transaction.Results;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly DisputePortalDbContext ctx;

    public TransactionRepository(DisputePortalDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<PagedResult<TransactionListItemDto>> GetPagedAsync(
        Guid customerId, Guid? accountId, TransactionFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var query = ctx.Transactions
            .AsNoTracking()
            .Where(t => t.CustomerId == customerId);

        if (accountId.HasValue)
            query = query.Where(t => t.AccountId == accountId.Value && t.Account.CustomerId == customerId);

        if (filters.DateFrom.HasValue)
            query = query.Where(t => t.TransactionDate >= filters.DateFrom.Value);
        if (filters.DateTo.HasValue)
            query = query.Where(t => t.TransactionDate <= filters.DateTo.Value);
        if (filters.Category.HasValue)
            query = query.Where(t => t.Category == filters.Category.Value);
        if (filters.AmountMin.HasValue)
            query = query.Where(t => t.Amount >= filters.AmountMin.Value);
        if (filters.AmountMax.HasValue)
            query = query.Where(t => t.Amount <= filters.AmountMax.Value);
        if (!string.IsNullOrWhiteSpace(filters.Search))
            query = query.Where(t => t.Description.Contains(filters.Search));

        query = filters.SortField.ToLower() switch
        {
            "amount"      => filters.SortDirection == "asc" ? query.OrderBy(t => t.Amount) : query.OrderByDescending(t => t.Amount),
            "description" => filters.SortDirection == "asc" ? query.OrderBy(t => t.Description) : query.OrderByDescending(t => t.Description),
            "category"    => filters.SortDirection == "asc" ? query.OrderBy(t => t.Category) : query.OrderByDescending(t => t.Category),
            _             => filters.SortDirection == "asc" ? query.OrderBy(t => t.TransactionDate) : query.OrderByDescending(t => t.TransactionDate)
        };

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TransactionListItemDto(
                t.Id, t.TransactionDate, t.Description, t.Amount,
                t.Category.ToString(), t.Reference, t.IsDisputed,
                t.Dispute != null ? t.Dispute.ReferenceNumber : null,
                t.AccountId, t.Account.AccountName, t.Account.AccountTypeId.ToString(), t.Account.AccountNumber
            ))
            .ToListAsync(ct);

        return new PagedResult<TransactionListItemDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<TransactionDetailDto?> GetByIdAsync(Guid id, Guid customerId, CancellationToken ct)
    {
        return await ctx.Transactions
            .AsNoTracking()
            .Where(t => t.Id == id && t.CustomerId == customerId)
            .Select(t => new TransactionDetailDto(
                t.Id, t.CustomerId, t.TransactionDate, t.Description, t.Amount,
                t.Category.ToString(), t.Reference, t.IsDisputed, t.CreatedAt,
                t.Dispute != null
                    ? new DisputeSummaryDto(t.Dispute.Id, t.Dispute.ReferenceNumber, t.Dispute.StatusId.ToString(), t.Dispute.SubmittedAt)
                    : null,
                new AccountInfoDto(t.AccountId, t.Account.AccountName, t.Account.AccountTypeId.ToString(), t.Account.AccountNumber)
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task SetDisputedAsync(Guid id, CancellationToken ct)
    {
        await ctx.Transactions
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsDisputed, true), ct);
    }
}
